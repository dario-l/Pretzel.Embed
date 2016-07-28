using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotLiquid;
using Pretzel.Logic.Extensibility;

namespace Pretzel.Embed
{
    [Export(typeof(ITag))]
    public class EmbedBlock : Block, ITag
    {
        private static readonly Regex TypeCleanRegex = new Regex(@"\W+>", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        protected string[] Params;

        public new string Name => "Embed";

        public string Type
        {
            get
            {
                if (Params.Any())
                    return Params[0];
                else
                    throw new InvalidOperationException("Embed type not specified.");
            }
        }

        public string Source
        {
            get
            {
                if (Params.Length > 1)
                    return Params[1];
                else
                    throw new InvalidOperationException("Embed source is not specified.");
            }
        }

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            Params = Regex.Matches(markup, @"[\""].+?[\""]|[^ ]+")
                          .OfType<Match>()
                          .Select(_ => (_.Value ?? String.Empty).Trim(' ', '"'))
                          .ToArray();

        }

        public override void Render(Context context, TextWriter result)
        {
            string type = TypeCleanRegex.Replace(Type.Trim().ToLowerInvariant(), "_");

            result.WriteLine(String.Format("<div class=\"embedded embedded-{0}\">", type));
            result.Write("    ");
            switch (type)
            {
                case "twitter":
                    RenderTwitter(context, result);
                    break;
                case "instagram":
                    RenderInstagram(context, result);
                    break;
                case "facebook":
                    RenderFacebook(context, result);
                    break;
                case "vk":
                    RenderVk(context, result);
                    break;
                case "youtube":
                    RenderYoutube(context, result);
                    break;
                default:
                    throw new InvalidOperationException(String.Format("Type \"{0}\" is not supported.", Type));
            }
            result.WriteLine("</div>");
        }

        protected string GetRenderedContent(Context context)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    base.Render(context, writer);
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        protected void RenderTwitter(Context context, TextWriter result)
        {
            result.WriteLine(String.Format("<blockquote class=\"twitter-tweet\" data-lang=\"pl\"><p lang=\"pl\" dir=\"ltr\">{0}</p><a href=\"{1}\"></a></blockquote>",
                GetRenderedContent(context), Source));
        }

        protected void RenderInstagram(Context context, TextWriter result)
        {
            result.WriteLine(String.Format("<blockquote class=\"instagram-media\" data-instgrm-captioned data-instgrm-version=\"6\" style=\" background:#FFF; border:0; border-radius:3px; box-shadow:0 0 1px 0 rgba(0,0,0,0.5),0 1px 10px 0 rgba(0,0,0,0.15); margin: 1px; max-width:658px; padding:0; width:99.375%; width:-webkit-calc(100% - 2px); width:calc(100% - 2px);\"><div style=\"padding:8px;\"> <div style=\" background:#F8F8F8; line-height:0; margin-top:40px; padding:50.0% 0; text-align:center; width:100%;\"> <div style=\" background:url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACwAAAAsCAMAAAApWqozAAAAGFBMVEUiIiI9PT0eHh4gIB4hIBkcHBwcHBwcHBydr+JQAAAACHRSTlMABA4YHyQsM5jtaMwAAADfSURBVDjL7ZVBEgMhCAQBAf//42xcNbpAqakcM0ftUmFAAIBE81IqBJdS3lS6zs3bIpB9WED3YYXFPmHRfT8sgyrCP1x8uEUxLMzNWElFOYCV6mHWWwMzdPEKHlhLw7NWJqkHc4uIZphavDzA2JPzUDsBZziNae2S6owH8xPmX8G7zzgKEOPUoYHvGz1TBCxMkd3kwNVbU0gKHkx+iZILf77IofhrY1nYFnB/lQPb79drWOyJVa/DAvg9B/rLB4cC+Nqgdz/TvBbBnr6GBReqn/nRmDgaQEej7WhonozjF+Y2I/fZou/qAAAAAElFTkSuQmCC); display:block; height:44px; margin:0 auto -44px; position:relative; top:-22px; width:44px;\"></div></div> <p style=\" margin:8px 0 0 0; padding:0 4px;\"> <a href=\"{1}\" style=\" color:#000; font-family:Arial,sans-serif; font-size:14px; font-style:normal; font-weight:normal; line-height:17px; text-decoration:none; word-wrap:break-word;\" target=\"_blank\">{0}</a></p></div></blockquote>",
                GetRenderedContent(context), Source));
        }

        protected void RenderFacebook(Context context, TextWriter result)
        {
            int height = 300;
            if (Params.Length > 2)
                int.TryParse(Params[2], out height);

            result.WriteLine(String.Format("<iframe src=\"https://www.facebook.com/plugins/post.php?href={0}&width=500\" width=\"500\" height=\"{1}\" style=\"border:none;overflow:hidden\" scrolling=\"no\" frameborder=\"0\" allowTransparency=\"true\"></iframe>",
                Source, height));
        }

        protected void RenderVk(Context context, TextWriter result)
        {
            result.WriteLine(String.Format("<div id=\"{0}\"></div><script type=\"text/javascript\">  (function(d, s, id) {{ var js, fjs = d.getElementsByTagName(s)[0]; if (d.getElementById(id)) return; js = d.createElement(s); js.id = id; js.src = \"//vk.com/js/api/openapi.js?121\"; fjs.parentNode.insertBefore(js, fjs); }}(document, 'script', 'vk_openapi_js'));  (function() {{    if (!window.VK || !VK.Widgets || !VK.Widgets.Post || !VK.Widgets.Post(\"{0}\", 2593705, 1050, 'XeccTh6esLIlcafEYa0MnXMf4g', {{width: 500}})) setTimeout(arguments.callee, 50);  }}());</script>",
                Source));
        }

        protected void RenderYoutube(Context context, TextWriter result)
        {
            result.WriteLine(String.Format("<div class=\"embed-container\"><iframe src=\"//www.youtube.com/embed/{0}\" frameborder=\"0\" allowfullscreen></iframe></div>",
                Source));
        }
    }
}
