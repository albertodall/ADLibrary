using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Windows.Forms.Controls
{
    class VEShape
    {
        private string _id;
        private string _title;

        public VEShape()
            : this(string.Empty, string.Empty)
        { }

        public VEShape(string id, string title)
        {
            _id = id;
            _title = title;
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
    }
}
