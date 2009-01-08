﻿var Administration =
{
    _lockUserUrl : '',
    _unlockUserUrl : '',

    set_lockUserUrl : function(value)
    {
        Administration._lockUserUrl = value;
    },

    set_unlockUserUrl : function(value)
    {
        Administration._unlockUserUrl = value;
    },

    set_blockUserUrl : function(value)
    {
        Administration._blockUserUrl = value;
    },

    set_unblockUserUrl : function(value)
    {
        Administration._unblockUserUrl = value;
    },

    init : function()
    {
        $('#frmPublish').submit (
                                    function()
                                    {
                                        var options =   {
                                                            dataType : 'json',
                                                            beforeSubmit : function(values, form, options)
                                                            {
                                                                $U.showProgress('Publishing Stories...');
                                                            },
                                                            success : function(result)
                                                            {
                                                                $U.hideProgress();

                                                                if (result.isSuccessful)
                                                                {
                                                                    $U.messageBox('Success', 'Story publishing process completed.', false);
                                                                }
                                                                else
                                                                {
                                                                    $U.messageBox('Error', result.errorMessage, true);
                                                                }
                                                            }
                                                        };

                                        $(this).ajaxSubmit(options); 
                                        return false;
                                    }
                                );

        $('#lnkChangeRole').click  (
                                        function()
                                        {
                                            $('#roleViewSection').hide();
                                            $('#roleEditSection').show();
                                            $U.focus('ddlRoles');
                                        }
                                    );

        $('#lnkCancelRole').click   (
                                        function()
                                        {
                                            $('#roleEditSection').hide();
                                            $('#roleViewSection').show();
                                        }
                                    );

        $('#frmChangeRole').submit (
                                    function()
                                    {
                                        var options =   {
                                                            dataType : 'json',
                                                            beforeSubmit : function(values, form, options)
                                                            {
                                                                $U.disableInputs('#roleEditSection', true);
                                                                $U.showProgress('Updating role...', '#btnChangeRole');
                                                            },
                                                            success : function(result)
                                                            {
                                                                $U.disableInputs('#roleEditSection', false);
                                                                $U.hideProgress();

                                                                if (result.isSuccessful)
                                                                {
                                                                    $('#roleEditSection').hide();
                                                                    $('#roleViewSection').show();

                                                                    var ddlRole = $('#ddlRoles')[0];
                                                                    $('#spnRole').text(ddlRole.options[ddlRole.selectedIndex].text);
                                                                }
                                                                else
                                                                {
                                                                    $U.messageBox('Error', result.errorMessage, true);
                                                                }
                                                            }
                                                        };

                                        $(this).ajaxSubmit(options); 
                                        return false;
                                    }
                                );

        $('#frmAllowIps').submit (
                                    function()
                                    {
                                        var options =   {
                                                            dataType : 'json',
                                                            beforeSubmit : function(values, form, options)
                                                            {
                                                                $U.showProgress('Allowing IpAddresses...', '#btnAllowIpAddresses');
                                                            },
                                                            success : function(result)
                                                            {
                                                                $U.hideProgress();

                                                                if (result.isSuccessful)
                                                                {
                                                                    $U.messageBox('Success', 'User Ip Addresses allowed.', false);
                                                                }
                                                                else
                                                                {
                                                                    $U.messageBox('Error', result.errorMessage, true);
                                                                }
                                                            }
                                                        };

                                        $(this).ajaxSubmit(options); 
                                        return false;
                                    }
                                );
    },

    lockUser : function(userId)
    {
        function lock()
        {
            var data = 'id=' + encodeURIComponent(userId);

            $.ajax  (
                        {
                            url : Administration._lockUserUrl,
                            type : 'POST',
                            dataType : 'json',
                            data : data,
                            beforeSend :    function()
                                            {
                                                $U.showProgress('Locking User...');
                                            },
                            success :   function(result)
                                        {
                                            $U.hideProgress();

                                            if (result.isSuccessful)
                                            {
                                                $('#lnkLockUser').hide();
                                                $('#lnkUnlockUser').show();
                                            }
                                            else
                                            {
                                                $U.messageBox('Error', result.errorMessage, true);
                                            }
                                        }
                        }
                    );
        }

        $U.confirm('Lock User?', 'Are you sure you want to lock this user?', lock);
    },

    unlockUser : function(userId)
    {
        function unlock()
        {
            var data = 'id=' + encodeURIComponent(userId);

            $.ajax  (
                        {
                            url : Administration._unlockUserUrl,
                            type : 'POST',
                            dataType : 'json',
                            data : data,
                            beforeSend :    function()
                                            {
                                                $U.showProgress('Unlocking User...');
                                            },
                            success :   function(result)
                                        {
                                            $U.hideProgress();

                                            if (result.isSuccessful)
                                            {
                                                $('#lnkUnlockUser').hide();
                                                $('#lnkLockUser').show();
                                            }
                                            else
                                            {
                                                $U.messageBox('Error', result.errorMessage, true);
                                            }
                                        }
                        }
                    );
        }

        $U.confirm('Unlock User?', 'Are you sure you want to unlock this user?', unlock);
    },

    dispose : function()
    {
    }
};