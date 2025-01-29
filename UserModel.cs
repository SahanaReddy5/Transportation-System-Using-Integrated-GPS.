using System;
using System.Collections.Generic;

namespace TransportSystemClient.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public int Id { get; set; }
    }
    public class UserSerialize
    {
        public object ContentEncoding { get; set; }
        public object ContentType { get; set; }
        public List<UserModel> Data { get; set; }
        public int JsonRequestBehavior { get; set; }
        public object MaxJsonLength { get; set; }
        public object RecursionLimit { get; set; }
    }
    public class UserLocation
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }
}
