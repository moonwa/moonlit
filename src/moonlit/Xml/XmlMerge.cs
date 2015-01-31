using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Moonlit.Xml
{
    public class XmlMerge
    {
        public static XElement Merge(XElementFinder finder, params XElement[] xElements)
        {
            if (xElements.Length == 0)
            {
                throw new Exception("there is no element in xElement");
            }
            XElement root = xElements[0];
            for (int i = 1; i < xElements.Length; i++)
            {
                Merge(root, xElements[i], finder);
            }
            return root;
        }

        private static void Merge(XElement xSrc, XElement xDst, XElementFinder finder)
        {
            foreach (var childDst in xDst.Elements())
            {
                var childSrc = finder(xSrc, childDst);

                if (childSrc == null)
                {
                    xSrc.Add(childDst);
                }
                else
                {
                    foreach (var attrDst in childDst.Attributes())
                    {
                        childSrc.SetAttributeValue(attrDst.Name, attrDst.Value);
                    }
                    Merge(childSrc, childDst, finder);
                }
            }
        }
    }

    public delegate XElement XElementFinder(XElement src, XElement element);
}
