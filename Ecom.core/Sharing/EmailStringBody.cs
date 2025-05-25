using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Sharing
{
    public class EmailStringBody
    {
        public static string send(string email, string token, string component, string message)
        {
            string encodeToken = Uri.EscapeDataString(token);
            return $@"
        <html>
            <head>
                <style>
                    .button {{
                        display: inline-block;
                        padding: 12px 24px;
                        font-size: 16px;
                        color: #fff;
                        background-color: #f82249;
                        text-decoration: none;
                        border-radius: 8px;
                        font-weight: bold;
                        margin-top: 20px;
                    }}
                    .button:hover {{
                        background-color: #c61c3a;
                    }}
                </style>
            </head>
            <body>
                <h1>{message}</h1>
                <hr>
                <br>
                <a class=""button"" href=""http://localhost:4200/Account/{component}?email={email}&code={encodeToken}"">
                    {message}
                </a>
            </body>
        </html>
    ";
        }

    }
}
