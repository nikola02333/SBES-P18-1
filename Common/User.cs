using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common
{
    [DataContract]
    public class User
    {
        public User()
        {}
        public User(string username, string pass, EnumType t )
        {
            Username = username;
            Pass = pass;
            Type =t;
        }
        private string  username;
        [DataMember]
        public string  Username
        {
            get { return username; }
            set { username = value; }
        }
        private string pass;
        [DataMember]
        public string Pass
        {
            get { return pass; }
            set { pass = value; }
        }
       
        private EnumType type;
        [DataMember]
        public EnumType Type
        {
            get { return type; }
            set { type = value; }
        }
        private bool isAuthenticated;
        [DataMember]
        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
            set { isAuthenticated = value; }
        }
       



    }
}
