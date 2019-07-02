namespace Sitecore.Spatial.GeoJson.Linq
{
    using System.IO;
    using Sitecore.ContentSearch.Linq.Common;

    internal class GenericDumpable : IDumpable
    {
        public GenericDumpable(object value)
        {
           this.Value = value;
        }

        protected object Value
        {
            get;
            set;
        }

        public virtual void WriteTo(TextWriter writer)
        {
            if (this.Value is IDumpable dumpable)
            {
                dumpable.WriteTo(writer);
            }
            else
            {
                writer.WriteLine(this.Value);
            }
        }
    }
}