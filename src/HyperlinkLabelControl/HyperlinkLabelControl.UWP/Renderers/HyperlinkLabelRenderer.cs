using HyperlinkLabelControl;
using HyperlinkLabelControl.UWP.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(HyperlinkLabel), typeof(HyperlinkLabelRenderer))]
namespace HyperlinkLabelControl.UWP.Renderers
{
    public class HyperlinkLabelRenderer : ViewRenderer<HyperlinkLabel, TextBlock>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<HyperlinkLabel> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != e.OldElement)
            {
                if (e.OldElement != null)
                    e.OldElement.PropertyChanged -= Element_PropertyChanged;

                if (e.NewElement != null)
                    e.NewElement.PropertyChanged += Element_PropertyChanged;
            }

            var tb = new TextBlock();
            tb.TextWrapping = Windows.UI.Xaml.TextWrapping.WrapWholeWords;
            SetNativeControl(tb);

            SetText();
        }

        private void SetText()
        {
            Control.Inlines.Clear();

            string text = Element.GetText(out List<HyperlinkLabelLink> links);
            if (text != null)
            {
                int index = 0;

                foreach (var item in links)
                {
                    Control.Inlines.Add(new Run() { Text = text.Substring(index, item.Start - index) });

                    var hl = new Hyperlink();
                    hl.NavigateUri = new Uri(item.Link);
                    hl.Inlines.Add(new Run() { Text = text.Substring(item.Start, item.Text.Length) });

                    Control.Inlines.Add(hl);

                    index = item.Start + item.Text.Length;

                    if(index < text.Length && item == links.LastOrDefault())
                        Control.Inlines.Add(new Run() { Text = text.Substring(index, text.Length - index) });
                }
            }
        }

        private void Element_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == HyperlinkLabel.RawTextProperty.PropertyName)
                SetText();
        }
    }
}
