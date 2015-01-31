using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Moonlit.Xml
{
    public class XmlComparer
    {
        private readonly List<IXmlIdentity> _xmlIdentities = new List<IXmlIdentity>();

        public XmlCompareResults Compare(string xmlSource, string xmlTagert)
        {
            XElement src = XElement.Parse(xmlSource);
            XElement dst = XElement.Parse(xmlTagert);
            var compareResults = new XmlCompareResults();
            if (src.Name != dst.Name)
            {
                compareResults.AddError(new TagDifferenceError(src, dst, "未找到指定元素"));
                return compareResults;
            }
            Compare(src, dst, compareResults);
            return compareResults;
        }

        private bool Compare(XElement src, XElement dst, XmlCompareResults compareResults)
        {
            if (!CompareAttributes(src, dst, compareResults) || !CompareAttributes(dst, src, compareResults))
                return false;
            if (!CompareChildren(src, dst, compareResults) || !CompareChildren(dst, src, compareResults))
                return false;

            foreach (XElement srcChild in src.Elements())
            {
                IXmlIdentity xmlIdentity = GetXmlIdentity(srcChild);
                XElement dstChild = dst.Elements().Where(x => xmlIdentity.IsSelf(x, srcChild)).FirstOrDefault();
                Compare(srcChild, dstChild, compareResults);
            }
            return true;
        }

        private bool CompareChildren(XElement src, XElement dst, XmlCompareResults compareResults)
        {
            bool compareValue = true;
            foreach (XElement srcChild in src.Elements())
            {
                IXmlIdentity xmlIdentity = GetXmlIdentity(srcChild);
                XElement dstChild = dst.Elements().Where(x => xmlIdentity.IsSelf(x, srcChild)).FirstOrDefault();
                if (dstChild == null)
                {
                    compareResults.AddError(new ElementNotExistError(src, dst, srcChild));
                    compareValue = false;
                }
            }
            return compareValue;
        }

        private IXmlIdentity GetXmlIdentity(XElement element)
        {
            foreach (IXmlIdentity xmlIdentity in _xmlIdentities)
            {
                if (xmlIdentity.TagName == element.Name)
                {
                    return xmlIdentity;
                }
            }
            return XmlTagIdentity.Instance;
        }

        private bool CompareAttributes(XElement src, XElement dst, XmlCompareResults compareResults)
        {
            bool compareValue = true;
            foreach (XAttribute srcAttr in src.Attributes())
            {
                XAttribute dstAttr = dst.Attributes(srcAttr.Name).FirstOrDefault();
                if (null == dstAttr)
                {
                    compareResults.AddError(new AttributeNotExist(src, dst, srcAttr.Name));
                    compareValue = false;
                }
                else if (srcAttr.Value != dstAttr.Value)
                {
                    compareResults.AddError(new AttributeNotEqual(src, dst, srcAttr.Name));
                    compareValue = false;
                }
            }
            return compareValue;
        }

        public void AddIdentify(IXmlIdentity xmlIdentity)
        {
            _xmlIdentities.Add(xmlIdentity);
        }
    }

    internal class AttributeNotEqual : IXmlCompareResult
    {
        private readonly XElement _element1;
        private readonly XElement _element2;
        private readonly XName _attrName;

        public AttributeNotEqual(XElement element1, XElement element2, XName attrName)
        {
            _element1 = element1;
            _element2 = element2;
            _attrName = attrName;
        }

        public XName AttrName
        {
            get { return _attrName; }
        }

        public XElement Element2
        {
            get { return _element2; }
        }

        public XElement Element1
        {
            get { return _element1; }
        }
    }

    internal class AttributeNotExist : IXmlCompareResult
    {
        private readonly XElement _element1;
        private readonly XElement _element2;
        private readonly XName _attrName;

        public AttributeNotExist(XElement element1, XElement element2, XName attrName)
        {
            _element1 = element1;
            _element2 = element2;
            _attrName = attrName;
        }

        public XName AttrName
        {
            get { return _attrName; }
        }

        public XElement Element2
        {
            get { return _element2; }
        }

        public XElement Element1
        {
            get { return _element1; }
        }
    }

    internal class ElementNotExistError : IXmlCompareResult
    {
        private readonly XElement _element1;
        private readonly XElement _element2;
        private readonly XElement _childElement;

        public ElementNotExistError(XElement element1, XElement element2, XElement childElement)
        {
            _element1 = element1;
            _element2 = element2;
            _childElement = childElement;
        }

        public XElement ChildElement
        {
            get { return _childElement; }
        }

        public XElement Element2
        {
            get { return _element2; }
        }

        public XElement Element1
        {
            get { return _element1; }
        }
    }

    public class TagDifferenceError : IXmlCompareResult
    {
        private readonly XElement _element1;
        private readonly XElement _element2;
        private readonly string _message;

        public TagDifferenceError(XElement element1, XElement element2, string message)
        {
            _element1 = element1;
            _element2 = element2;
            _message = message;
        }

        public string Message
        {
            get { return _message; }
        }

        public XElement Element2
        {
            get { return _element2; }
        }

        public XElement Element1
        {
            get { return _element1; }
        }
    }

    internal class XmlTagIdentity : IXmlIdentity
    {
        internal static readonly XmlTagIdentity Instance = new XmlTagIdentity();

        private XmlTagIdentity()
        {
        }

        #region IXmlIdentity Members

        public XName TagName
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSelf(XElement x, XElement y)
        {
            return x.Name == y.Name;
        }

        #endregion
    }

    public interface IXmlIdentity
    {
        XName TagName { get; }
        bool IsSelf(XElement x, XElement y);
    }


    public class AttributeIdentify : IXmlIdentity
    {
        private readonly string _attrName;
        private readonly string _tagName;
        private readonly string _namespace;

        public AttributeIdentify(string tagName, string attrName)
            : this(tagName, attrName, "")
        {
        }

        public AttributeIdentify(string tagName, string attrName, string @namespace)
        {
            _tagName = tagName;
            _attrName = attrName;
            if (!string.IsNullOrEmpty(@namespace))
                _namespace = @namespace;
        }

        #region IXmlIdentity Members

        public string NameSpace
        {
            get { return _namespace; }
        }

        public XName TagName
        {
            get
            {
                if (!string.IsNullOrEmpty(_namespace))
                    return XName.Get(_tagName, _namespace);
                return _tagName;
            }
        }

        public bool IsSelf(XElement x, XElement y)
        {
            XAttribute xAttr = x.Attribute(_attrName);
            XAttribute yAttr = y.Attribute(_attrName);
            return xAttr != null && yAttr != null && xAttr.Value == yAttr.Value;
        }

        #endregion
    }


    [Serializable]
    public class XmlCompareResults
    {
        private readonly List<IXmlCompareResult> _errors = new List<IXmlCompareResult>();

        public bool IsValid
        {
            get { return _errors.Count == 0; }
        }

        public IEnumerable<IXmlCompareResult> Errors
        {
            get { return _errors; }
        }

        public void AddError(IXmlCompareResult xmlCompareResult)
        {
            _errors.Add(xmlCompareResult);
        }
    }

    public interface IXmlCompareResult
    {
        XElement Element2 { get; }
        XElement Element1 { get; }
    }
}