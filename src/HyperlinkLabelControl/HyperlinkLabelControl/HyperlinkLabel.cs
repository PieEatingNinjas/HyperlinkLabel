using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace HyperlinkLabelControl
{
    public class HyperlinkLabel : Label
    {
        public string RawText
        {
            get => (string)GetValue(RawTextProperty);
            set => SetValue(RawTextProperty, value);
        }

        public static readonly BindableProperty RawTextProperty =
            BindableProperty.Create(nameof(RawText), typeof(string), typeof(HyperlinkLabel), null);

        public string GetText(out List<HyperlinkLabelLink> links)
        {
            links = new List<HyperlinkLabelLink>();
            if (RawText == null)
                return null;

            string pattern = @"\[([^]]*)\]\(([^\s^\)]*)\)";

            var linksInText = Regex.Matches(RawText, pattern, RegexOptions.IgnoreCase);

            string Text = RawText;

            for (int i = 0; i < linksInText.Count; i++)
            {
                string fullMatch = linksInText[i].Groups[0].Value;
                string text = linksInText[i].Groups[1].Value;
                string link = linksInText[i].Groups[2].Value;

                int start = Text.IndexOf(fullMatch);

                if (start > -1)
                {
                    Text =
                        $"{Text.Substring(0, start)}{text}{Text.Substring(start + fullMatch.Length)}";
                    links.Add(new HyperlinkLabelLink(text, link, start));
                }
            }
            return Text;
        }
    }

    public class HyperlinkLabelLink
    {
        internal HyperlinkLabelLink(string text, string link, int start)
        {
            Text = text;
            Link = link;
            Start = start;
        }

        public string Text { get; }
        public string Link { get; }
        public int Start { get; }
    }
}
