namespace Kigg.Web
{
    using System;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.IO;
    using System.Web;

    using DomainObjects;
    using Infrastructure;
    using Repository;

    public class ImageHandler : BaseHandler
    {
        public const string DefaultBorderColor = "808080";
        public const string DefaultTextBackColor = "404040";
        public const string DefaultTextForeColor = "ffffff";
        public const string DefaultCountBackColor = "eb4c07";
        public const string DefaultCountForeColor = "ffffff";

        public const int DefaultWidth = 100;
        public const int DefaultHeight = 22;
        public const int DefaultBorderWidth = 1;
        public const string DefaultFontName = "Tahoma";
        public const int DefaultFontSizeInPixel = 12;

        private const int DefaultCacheDurationInMinutes = 5;

        public ImageHandler()
        {
            IoC.Inject(this);
        }

        public IConfigurationSettings Settings
        {
            get;
            set;
        }

        public IStoryRepository StoryRepository
        {
            get;
            set;
        }

        public static Color GetColor(NameValueCollection queryString, string key, string defaultValue)
        {
            string hexValue = string.IsNullOrEmpty(queryString[key]) ? defaultValue : queryString[key];

            if (!hexValue.StartsWith("#", StringComparison.OrdinalIgnoreCase))
            {
                hexValue = "#" + hexValue;
            }

            try
            {
                return ColorTranslator.FromHtml(hexValue);
            }
            catch
            {
                return ColorTranslator.FromHtml(defaultValue);
            }
        }

        public static int GetInteger(NameValueCollection queryString, string key, int defaultValue)
        {
            int value = defaultValue;

            if (queryString[key] != null)
            {
                if (!int.TryParse(queryString[key], out value))
                {
                    value = defaultValue;
                }
            }

            return value;
        }

        public override void ProcessRequest(HttpContextBase context)
        {
            const int CountWidthBuffer = 6;

            HttpRequestBase request = context.Request;
            string url = request.QueryString["url"];

            Color borderColor = GetColor(request.QueryString, "borderColor", DefaultBorderColor);
            Color textBackColor = GetColor(request.QueryString, "textBackColor", DefaultTextBackColor);
            Color textForeColor = GetColor(request.QueryString, "textForeColor", DefaultTextForeColor);
            Color countBackColor = GetColor(request.QueryString, "countBackColor", DefaultCountBackColor);
            Color countForeColor = GetColor(request.QueryString, "countForeColor", DefaultCountForeColor);

            int borderWidth = GetInteger(request.QueryString, "borderWidth", DefaultBorderWidth);
            string fontName = request.QueryString["fontName"] ?? DefaultFontName;
            int fontSize = GetInteger(request.QueryString, "fontSize", DefaultFontSizeInPixel);
            int width = GetInteger(request.QueryString, "width", DefaultWidth);
            int height = GetInteger(request.QueryString, "height", DefaultHeight);

            HttpResponseBase response = context.Response;
            DateTime storyLastActivityAt = SystemTime.Now();

            using (MemoryStream ms = new MemoryStream())
            {
                using (Image image = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                {
                    using (Graphics gdi = Graphics.FromImage(image))
                    {
                        gdi.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                        gdi.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gdi.SmoothingMode = SmoothingMode.HighSpeed;
                        gdi.CompositingQuality = CompositingQuality.HighSpeed;

                        using (Brush borderBrush = new SolidBrush(borderColor))
                        {
                            gdi.FillRectangle(borderBrush, 0, 0, image.Width, image.Height);
                        }

                        using (Brush textBackgroundBrush = new SolidBrush(textBackColor))
                        {
                            gdi.FillRectangle(textBackgroundBrush, borderWidth, borderWidth, (image.Width - (borderWidth * 2)), (image.Height - (borderWidth * 2)));
                        }

                        int count = 0;

                        if (url.IsWebUrl())
                        {
                            IStory story = StoryRepository.FindByUrl(url);

                            if (story != null)
                            {
                                count = story.VoteCount;
                                storyLastActivityAt = story.LastActivityAt;
                            }
                        }

                        using (Font font = new Font(fontName, fontSize, FontStyle.Bold, GraphicsUnit.Pixel))
                        {
                            SizeF countSize = gdi.MeasureString(count.ToString(Constants.CurrentCulture), font);

                            float textWidth = (image.Width - (countSize.Width + CountWidthBuffer + (borderWidth * 2)));

                            using (Brush textForegroundBrush = new SolidBrush(textForeColor))
                            {
                                SizeF textSize = gdi.MeasureString(Settings.PromoteText, font);

                                float x = (((textWidth - textSize.Width) / 2) + borderWidth);
                                float y = ((image.Height - textSize.Height) / 2);

                                gdi.DrawString(Settings.PromoteText, font, textForegroundBrush, x, y);
                            }

                            using (Brush countBackgroundBrush = new SolidBrush(countBackColor))
                            {
                                gdi.FillRectangle(countBackgroundBrush, (textWidth + borderWidth), borderWidth, (countSize.Width + CountWidthBuffer), (image.Height - (borderWidth * 2)));
                            }

                            using (Brush countForegroundBrush = new SolidBrush(countForeColor))
                            {
                                float x = ((((countSize.Width + CountWidthBuffer) - countSize.Width) / 2) + borderWidth + textWidth);
                                float y = ((image.Height - countSize.Height) / 2);

                                gdi.DrawString(count.ToString(Constants.CurrentCulture), font, countForegroundBrush, x, y);
                            }
                        }
                    }

                    image.Save(ms, ImageFormat.Png);
                }

                ms.WriteTo(response.OutputStream);
                response.ContentType = "image/PNG";
            }

            bool doNotCache;

            if (!bool.TryParse(request.QueryString["noCache"], out doNotCache))
            {
                doNotCache = false;
            }

            if (doNotCache)
            {
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            }
            else
            {
                // If Story is older than 7 days then cache the it for 1 week otherwise 5 minutes
                int durationInMinute = ((SystemTime.Now() - storyLastActivityAt).Days > 7) ? (60 * 24 * 7) : DefaultCacheDurationInMinutes;

                if (durationInMinute > 0)
                {
                    context.CacheResponseFor(TimeSpan.FromMinutes(durationInMinute));
                }
            }
        }
    }
}