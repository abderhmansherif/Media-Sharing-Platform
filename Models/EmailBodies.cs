using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeatBox.Models
{
    public static class EmailBodies
    {
        public static string ForgotPasswordPlainTextContent = @"Hello {{UserName}},

                                                                We received a request to reset your password for your BeatBox account.
                                                                To reset your password, please click the link below or copy and paste it into your browser:

                                                                {{ResetLink}}

                                                                If you didn’t request a password reset, please ignore this email.
      
                                                                Best regards,
                                                                The BeatBox Team";


        public static string ForgotPasswordHtmlContent = @"
            <!doctype html>
            <html>
            <head>
                <meta charset='utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1'>
                <style>
                body { margin:0; padding:0; font-family: 'Poppins', Arial, sans-serif; background:#f4f6f8; }
                .container { max-width:600px; margin:24px auto; background:#ffffff; border-radius:8px; overflow:hidden; box-shadow:0 6px 18px rgba(0,0,0,0.06); }
                .header { padding:20px; text-align:center; background: linear-gradient(90deg,#6c5ce7,#2575fc); color:#fff; }
                .brand { font-size:22px; font-weight:700; }
                .content { padding:28px 32px; color:#2d3436; line-height:1.6; }
                .btn { display:inline-block; padding:12px 20px; border-radius:6px; text-decoration:none; font-weight:600; }
                .btn-primary { background:#fd79a8; color:#fff; }
                .small { font-size:13px; color:#636e72; }
                .footer { background:#fafafa; padding:16px 32px; font-size:13px; color:#636e72; text-align:center; }
                @media (max-width:480px) {
                    .content { padding:18px; }
                    .header { padding:16px; }
                }
                </style>
            </head>
            <body>
                <table role='presentation' width='100%' style='border-collapse:collapse;'>
                <tr>
                    <td align='center'>
                    <table role='presentation' class='container' width='100%' style='border-collapse:collapse;'>
                        <tr>
                        <td class='header'>
                            <div class='brand'>🎵 BeatBox</div>
                            <h1>Password Reset</h1>
                        </td>
                        </tr>

                        <tr>
                        <td class='content'>
                            <p>Hello {{UserName}},</p>

                            <p>We received a request to reset the password for your <strong>BeatBox</strong> account.  
                            Click the button below to create a new password. This link will expire in <strong>60 minutes</strong>.</p>

                            <p style='text-align:center; margin:22px 0;'>
                            <a href='{{ResetLink}}' class='btn btn-primary' target='_blank' rel='noopener'>Reset Your Password</a>
                            </p>

                            <p class='small'>If the button above doesn’t work, copy and paste this link into your browser:</p>
                            <p class='small'><a href='{{ResetLink}}' target='_blank' rel='noopener'>{{ResetLink}}</a></p>

                            <hr style='border:none; border-top:1px solid #eef2f7; margin:20px 0;' />

                            <p class='small'>If you did not request this password reset, please ignore this email.</p>

                            <p class='small' style='margin-top:10px;'>Thanks,<br/>The BeatBox Team</p>
                        </td>
                        </tr>

                        <tr>
                        <td class='footer'>
                            <div>© BeatBox. All rights reserved.</div>
                        </td>
                        </tr>

                    </table>
                    </td>
                </tr>
                </table>
            </body>
            </html>
            ";


        public static string ConfirmEmailPlainText = @"Hello {{UserName}},
                                                        We received a request to Confirm your email for your BeatBox account.
                                                        To confirm your email, please click the link below or copy and paste it into your browser:

                                                        {{ResetLink}}

                                                        If you didn’t request a Email Confirmation, please ignore this email.

                                                        Best regards,
                                                        The BeatBox Team";


        public static string ConfirmEmailHtmlBody = @"
            <!doctype html>
            <html>
            <head>
                <meta charset='utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1'>
                <style>
                body { margin:0; padding:0; font-family: 'Poppins', Arial, sans-serif; background:#f4f6f8; }
                .container { max-width:600px; margin:24px auto; background:#ffffff; border-radius:8px; overflow:hidden; box-shadow:0 6px 18px rgba(0,0,0,0.06); }
                .header { padding:20px; text-align:center; background: linear-gradient(90deg,#6c5ce7,#2575fc); color:#fff; }
                .brand { font-size:22px; font-weight:700; }
                .content { padding:28px 32px; color:#2d3436; line-height:1.6; }
                .btn { display:inline-block; padding:12px 20px; border-radius:6px; text-decoration:none; font-weight:600; }
                .btn-primary { background:#fd79a8; color:#fff; }
                .small { font-size:13px; color:#636e72; }
                .footer { background:#fafafa; padding:16px 32px; font-size:13px; color:#636e72; text-align:center; }
                @media (max-width:480px) {
                    .content { padding:18px; }
                    .header { padding:16px; }
                }
                </style>
            </head>
            <body>
                <table role='presentation' width='100%' style='border-collapse:collapse;'>
                <tr>
                    <td align='center'>
                    <table role='presentation' class='container' width='100%' style='border-collapse:collapse;'>
                        <tr>
                        <td class='header'>
                            <div class='brand'>🎵 BeatBox</div>
                            <h1>Confirm Your Email</h1>
                        </td>
                        </tr>

                        <tr>
                        <td class='content'>
                            <p>Hello {{UserName}},</p>

                            <p>We received a request to confirm your email for your <strong>BeatBox</strong> account.  
                            Click the button below to confirm your email. This link will expire in <strong>60 minutes</strong>.</p>

                            <p style='text-align:center; margin:22px 0;'>
                            <a href='{{ResetLink}}' class='btn btn-primary' target='_blank' rel='noopener'>Confirm Your Email</a>
                            </p>

                            <p class='small'>If the button above doesn’t work, copy and paste this link into your browser:</p>
                            <p class='small'><a href='{{ResetLink}}' target='_blank' rel='noopener'>{{ResetLink}}</a></p>

                            <hr style='border:none; border-top:1px solid #eef2f7; margin:20px 0;' />

                            <p class='small'>If you did not request this password reset, please ignore this email.</p>

                            <p class='small' style='margin-top:10px;'>Thanks,<br/>The BeatBox Team</p>
                        </td>
                        </tr>

                        <tr>
                        <td class='footer'>
                            <div>© BeatBox. All rights reserved.</div>
                        </td>
                        </tr>

                    </table>
                    </td>
                </tr>
                </table>
            </body>
            </html>
            ";




        public static string TwoFactorEnabledHtmlBody = @"
            <!doctype html>
            <html>
            <head>
                <meta charset='utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1'>
                <style>
                    body { margin:0; padding:0; font-family: 'Poppins', Arial, sans-serif; background:#f4f6f8; }
                    .container { max-width:600px; margin:24px auto; background:#ffffff; border-radius:8px; overflow:hidden; box-shadow:0 6px 18px rgba(0,0,0,0.06); }
                    .header { padding:20px; text-align:center; background: linear-gradient(90deg,#00b894,#00cec9); color:#fff; }
                    .brand { font-size:22px; font-weight:700; }
                    .content { padding:28px 32px; color:#2d3436; line-height:1.6; }
                    .btn { display:inline-block; padding:12px 20px; border-radius:6px; text-decoration:none; font-weight:600; }
                    .btn-primary { background:#0984e3; color:#fff; }
                    .small { font-size:13px; color:#636e72; }
                    .footer { background:#fafafa; padding:16px 32px; font-size:13px; color:#636e72; text-align:center; }
                    @media (max-width:480px) {
                        .content { padding:18px; }
                        .header { padding:16px; }
                    }
                </style>
            </head>
            <body>
                <table role='presentation' width='100%' style='border-collapse:collapse;'>
                    <tr>
                        <td align='center'>
                            <table role='presentation' class='container' width='100%' style='border-collapse:collapse;'>
                                <tr>
                                    <td class='header'>
                                        <div class='brand'>🎵 BeatBox</div>
                                        <h1>Two-Factor Authentication Enabled</h1>
                                    </td>
                                </tr>

                                <tr>
                                    <td class='content'>
                                        <p>Hello {{UserName}},</p>

                                        <p>We wanted to let you know that <strong>Two-Factor Authentication (2FA)</strong> has been successfully enabled for your <strong>BeatBox</strong> account.</p>

                                        <p>This means your account is now protected with an extra layer of security.  
                                        Each time you log in, you’ll need to verify your identity using your second factor (like an email or an authenticator code).</p>

                                        <p class='small'>If you didn’t enable this feature yourself, please contact our support team immediately.</p>

                                        <hr style='border:none; border-top:1px solid #eef2f7; margin:20px 0;' />

                                        <p class='small' style='margin-top:10px;'>Thanks for keeping your account secure,<br/>The BeatBox Team</p>
                                    </td>
                                </tr>

                                <tr>
                                    <td class='footer'>
                                        <div>© BeatBox. All rights reserved.</div>
                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>
            ";





        public static string TwoFactorEnabledPlainText = @"
                    Hello {{UserName}},

                    Two-Factor Authentication (2FA) has been successfully enabled for your BeatBox account.

                    Your account is now protected with an extra layer of security. 
                    From now on, you’ll need to verify your identity each time you log in using your second factor (like an email or authenticator code).

                    If you didn’t enable this feature yourself, please contact our support team immediately.

                    Thanks,
                    The BeatBox Team
                    ";

        public static string TwoFactorDisabledHtmlBody = @"
                <!doctype html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1'>
                    <style>
                        body { margin:0; padding:0; font-family: 'Poppins', Arial, sans-serif; background:#f4f6f8; }
                        .container { max-width:600px; margin:24px auto; background:#ffffff; border-radius:8px; overflow:hidden; box-shadow:0 6px 18px rgba(0,0,0,0.06); }
                        .header { padding:20px; text-align:center; background: linear-gradient(90deg,#ff7675,#d63031); color:#fff; }
                        .brand { font-size:22px; font-weight:700; }
                        .content { padding:28px 32px; color:#2d3436; line-height:1.6; }
                        .btn { display:inline-block; padding:12px 20px; border-radius:6px; text-decoration:none; font-weight:600; }
                        .btn-primary { background:#0984e3; color:#fff; }
                        .small { font-size:13px; color:#636e72; }
                        .footer { background:#fafafa; padding:16px 32px; font-size:13px; color:#636e72; text-align:center; }
                        @media (max-width:480px) {
                            .content { padding:18px; }
                            .header { padding:16px; }
                        }
                    </style>
                </head>
                <body>
                    <table role='presentation' width='100%' style='border-collapse:collapse;'>
                        <tr>
                            <td align='center'>
                                <table role='presentation' class='container' width='100%' style='border-collapse:collapse;'>
                                    <tr>
                                        <td class='header'>
                                            <div class='brand'>🎵 BeatBox</div>
                                            <h1>Two-Factor Authentication Disabled</h1>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class='content'>
                                            <p>Hello {{UserName}},</p>

                                            <p>This is to inform you that <strong>Two-Factor Authentication (2FA)</strong> has been <strong>disabled</strong> on your <strong>BeatBox</strong> account.</p>

                                            <p>This means your account is no longer protected by the extra layer of security provided by 2FA.  
                                            If you disabled this feature yourself, no further action is needed.</p>

                                            <p class='small'>⚠️ However, if you didn’t make this change, your account may be at risk.  
                                            Please log in immediately and re-enable Two-Factor Authentication to protect your data.</p>

                                            <hr style='border:none; border-top:1px solid #eef2f7; margin:20px 0;' />

                                            <p class='small' style='margin-top:10px;'>Stay safe,<br/>The BeatBox Security Team</p>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class='footer'>
                                            <div>© BeatBox. All rights reserved.</div>
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>
                ";


        public static string TwoFactorDisabledPlainText = @"
                Hello {{UserName}},

                Two-Factor Authentication (2FA) has been disabled on your BeatBox account.

                Your account is now less secure because the additional verification layer has been removed.

                If you disabled this feature yourself, no further action is required.
                But if you didn’t, your account might be compromised. Please log in and re-enable 2FA immediately.

                Stay safe,
                The BeatBox Security Team
                ";

        public static string TwoFactorOtpHtmlBody = @"
                    <!doctype html>
                    <html>
                    <head>
                        <meta charset='utf-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1'>
                        <style>
                            body { margin:0; padding:0; font-family: 'Poppins', Arial, sans-serif; background:#f4f6f8; }
                            .container { max-width:600px; margin:24px auto; background:#ffffff; border-radius:8px; overflow:hidden; box-shadow:0 6px 18px rgba(0,0,0,0.06); }
                            .header { padding:20px; text-align:center; background: linear-gradient(90deg,#6c5ce7,#2575fc); color:#fff; }
                            .brand { font-size:22px; font-weight:700; }
                            .content { padding:28px 32px; color:#2d3436; line-height:1.6; text-align:center; }
                            .otp-box { display:inline-block; padding:14px 24px; background:#f1f3f5; border-radius:8px; font-size:28px; font-weight:700; letter-spacing:4px; color:#2d3436; margin:16px 0; }
                            .small { font-size:13px; color:#636e72; text-align:center; }
                            .footer { background:#fafafa; padding:16px 32px; font-size:13px; color:#636e72; text-align:center; }
                            @media (max-width:480px) {
                                .content { padding:18px; }
                                .header { padding:16px; }
                                .otp-box { font-size:22px; }
                            }
                        </style>
                    </head>
                    <body>
                        <table role='presentation' width='100%' style='border-collapse:collapse;'>
                            <tr>
                                <td align='center'>
                                    <table role='presentation' class='container' width='100%' style='border-collapse:collapse;'>
                                        <tr>
                                            <td class='header'>
                                                <div class='brand'>🎵 BeatBox</div>
                                                <h1>Your One-Time Password (OTP)</h1>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class='content'>
                                                <p>Hello {{UserName}},</p>

                                                <p>Here’s your One-Time Password (OTP) for verifying your identity on <strong>BeatBox</strong>.</p>

                                                <div class='otp-box'>{{OTPCode}}</div>

                                                <p class='small'>This code will expire in <strong>5 minutes</strong>.  
                                                If you didn’t request this code, you can safely ignore this email.</p>

                                                <p class='small' style='margin-top:10px;'>Stay secure,<br/>The BeatBox Security Team</p>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class='footer'>
                                                <div>© BeatBox. All rights reserved.</div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </body>
                    </html>
                    ";

        

        public static string TwoFactorOtpPlainText = @"
                Hello {{UserName}},

                Here’s your One-Time Password (OTP) for verifying your identity on BeatBox.

                Your OTP code is: {{OTPCode}}

                This code will expire in 5 minutes.

                If you didn’t request this code, you can safely ignore this email.

                Stay secure,
                The BeatBox Security Team
                ";







    
    }
}