using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace gh_tcp_conn
{
    public class gh_tcp_connInfo : GH_AssemblyInfo
    {
        public override string Name => "tcp_connnection";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("951EF0AB-E171-49B0-A0CD-358AE9CEE94F");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}