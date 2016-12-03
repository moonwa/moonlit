using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Moonlit.Weixin
{
    /*
     <xml>
 <ToUserName><![CDATA[toUser]]></ToUserName>
 <FromUserName><![CDATA[fromUser]]></FromUserName>
 <CreateTime>12345678</CreateTime>
 <MsgType><![CDATA[text]]></MsgType>
 <Content><![CDATA[你好]]></Content>
 </xml>
     */
    public class TextMessage : IRequestMessage, IResponseMessage
    {
        public string ToUserName { get; set; }

        public string FromUserName { get; set; }
        public DateTime CreateTime { get; set; }
        public string Content { get; set; }

        public XElement ToXml()
        {
            return new XElement("xml", CreateXml());
        }

        public void FromXml(XElement element)
        {
            this.ToUserName = (string)element.Element("ToUserName");
            this.FromUserName = (string)element.Element("FromUserName");
            this.Content = (string)element.Element("Content");
            this.CreateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((int)element.Element("CreateTime")).ToLocalTime();
        }

        private object[] CreateXml()
        {
            return new[]
            {
                new XElement("ToUserName", this.ToUserName),
                new XElement("FromUserName", this.FromUserName),
                new XElement("CreateTime", (this.CreateTime  - new DateTime(1970, 1, 1)) .TotalSeconds),
                new XElement("MsgType", "text"),
                new XElement("Content", this.Content),
            };
        }
    }
    /*
    <xml><ToUserName><![CDATA[gh_e136c6e50636]]></ToUserName>
<FromUserName><![CDATA[oMgHVjngRipVsoxg6TuX3vz6glDg]]></FromUserName>
<CreateTime>1408090651</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[pic_sysphoto]]></Event>
<EventKey><![CDATA[6]]></EventKey>
<SendPicsInfo><Count>1</Count>
<PicList><item><PicMd5Sum><![CDATA[1b5f7c23b5bf75682a53e7b6d163e185]]></PicMd5Sum>
</item>
</PicList>
</SendPicsInfo>
</xml>
     */
    public class PhotoEventMessageBase : IRequestMessage
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public DateTime CreateTime { get; set; }
        public List<PhotoMessageInfo> Pictures { get; set; }
        public string EventKey { get; set; }

        public void FromXml(XElement element)
        {
            this.ToUserName = (string)element.Element("ToUserName");
            this.FromUserName = (string)element.Element("FromUserName");
            this.EventKey = (string)element.Element("EventKey");
            this.CreateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((int)element.Element("CreateTime")).ToLocalTime();
            //var count = (int) element.Element("SendPicsInfo").Element("Count");
            Pictures = new List<PhotoMessageInfo>();
            var picListElement = element.Element("SendPicsInfo").Element("PicList");
            foreach (var e in picListElement.Elements("item"))
            {
                Pictures.Add(new PhotoMessageInfo()
                {
                    Md5 = (string)e.Element("PicMd5Sum"),
                });
            }
        }
    }
    /*
    <xml>
                <ToUserName><![CDATA[gh_ee234cfa54cd]]></ToUserName>
                <FromUserName><![CDATA[oNm45xK4rQ82C2Z5mHmar5qrq2ew]]></FromUserName>
                <CreateTime>1447517266</CreateTime>
                <MsgType><![CDATA[image]]></MsgType>
                <PicUrl><![CDATA[http://mmbiz.qpic.cn/mmbiz/ohGfkhF6WDymzYJFK3Qib5fwYPtBUJ4dyO5CjWtkOGIUhZHrejzdmc70ueCiaLvUbWBicdOQV8sJhibfkVrUmHu2ZQ/0]]></PicUrl>
                <MsgId>6217039318265774317</MsgId>
                <MediaId><![CDATA[hrJzzphu6KiUKfFx32hFOCOZfCORV77opcq4c_Igog-KNyaKo_-KyZ8UJ0rpmuDe]]></MediaId>
</xml>
     */
    public class ImageMessage : IRequestMessage
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public DateTime CreateTime { get; set; }
        public string PicUrl { get; set; }
        public string MediaId { get; set; }

        public void FromXml(XElement element)
        {
            this.ToUserName = (string)element.Element("ToUserName");
            this.FromUserName = (string)element.Element("FromUserName");
            this.CreateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((int)element.Element("CreateTime")).ToLocalTime();
            this.PicUrl = (string)element.Element("PicUrl");
            this.MediaId = (string)element.Element("MediaId");
        }
    }

    public class PhotoEventMessage : PhotoEventMessageBase
    {
        
    }
    /*
    <xml><ToUserName><![CDATA[gh_e136c6e50636]]></ToUserName>
<FromUserName><![CDATA[oMgHVjngRipVsoxg6TuX3vz6glDg]]></FromUserName>
<CreateTime>1408090651</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[pic_photo_or_album]]></Event>
<EventKey><![CDATA[6]]></EventKey>
<SendPicsInfo><Count>1</Count>
<PicList><item><PicMd5Sum><![CDATA[1b5f7c23b5bf75682a53e7b6d163e185]]></PicMd5Sum>
</item>
</PicList>
</SendPicsInfo>
</xml>
     */
    public class PhotoAlbumEventMessage : PhotoEventMessageBase
    {
        
    } 
    public class LocationEventMessage : IRequestMessage
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public DateTime CreateTime { get; set; } 
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Precision { get; set; }

        public void FromXml(XElement element)
        {
            this.ToUserName = (string)element.Element("ToUserName");
            this.FromUserName = (string)element.Element("FromUserName");
            this.CreateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((int)element.Element("CreateTime")).ToLocalTime();
            Latitude = (decimal) element.Element("Latitude");
            Longitude = (decimal) element.Element("Longitude");
            Precision = (decimal) element.Element("Precision");
         
        } 
    } 
    public class PhotoMessageInfo
    {
        public string Md5 { get; set; }
    }
}
