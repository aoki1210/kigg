﻿var Faq =
{
    init : function()
    {
        $('#faq').find('a.q').each(
                                    function()
                                    {
                                        $(this).click(
                                                        function()
                                                        {
                                                            $(this).next('div.ans').toggle('normal');
                                                        }
                                                    )
                                    }
                       );
    },

    dispose : function()
    {
    }
};