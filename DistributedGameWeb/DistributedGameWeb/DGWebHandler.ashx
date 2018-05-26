<%@ WebHandler Language="C#" Class="DGWebHandler" %>

using System;
using System.Web;

public class DGWebHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write("Hello World");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}