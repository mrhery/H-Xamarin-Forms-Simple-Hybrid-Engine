using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace HXFSimpleHybrid.Core
{
    public class HHttpServer
    {

        static HttpListener server;
        public static string Dir = "WWW";
        static string names = typeof(App).Namespace;
        public static bool IsLitening
        {
            get
            {
                return server.IsListening;
            }
        }

        public static bool IsRunning = false;

        public static void Init(string port = "45100")
        {
            Config.port = port;
            server = new HttpListener();
            server.Prefixes.Add("http://127.0.0.1:" + Config.port + @"/");
            server.Prefixes.Add("http://127.0.0.1:" + Config.port + @"/");
        }



        public static async void Start()
        {
            server.Start();

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(HHttpServer)).Assembly;
            IsRunning = true;

            while (server.IsListening)
            {
                HttpListenerContext context = server.GetContext();
                HttpListenerResponse response = context.Response;
                HttpListenerRequest request = context.Request;

                string page = context.Request.Url.LocalPath;

                try
                {
                    string[] route = page.Split(new char[] { '/' });

                    if (route.Length > 1)
                    {
                        if (route[1] == "sqldb")
                        {
                            switch (route[2])
                            {
                                case "query":
                                    var yx = Input.Deserialize(Input.ReadStream(request.InputStream, request.ContentEncoding));

                                    var data = Tables.TableRoute.Run(yx["table"], yx["sql"]);

                                    dynamic res = new ExpandoObject();
                                    res.status = "success";
                                    res.code = "QUERY_EXECUTED";
                                    res.message = "Requested query has been executed.";
                                    res.data = data;

                                    string json = JsonConvert.SerializeObject(res);
                                    byte[] buffer1 = Encoding.UTF8.GetBytes(json);

                                    response.ContentLength64 = buffer1.Length;
                                    Stream stc1 = response.OutputStream;
                                    stc1.Write(buffer1, 0, buffer1.Length);
                                    stc1.Close();
                                    break;

                                default:
                                    dynamic x = new ExpandoObject();
                                    x.status = "error";
                                    x.code = "SQLDB_ENDPOINT_NOT_EXISTS";
                                    x.message = "Requested endpoint SQLDB not exitst.";

                                    string msgc = JsonConvert.SerializeObject(x);
                                    byte[] bufferc = Encoding.UTF8.GetBytes(msgc);

                                    response.ContentLength64 = bufferc.Length;
                                    Stream stc = response.OutputStream;
                                    stc.Write(bufferc, 0, bufferc.Length);
                                    break;
                            }
                        }
                        else
                        {
                            switch (route[1])
                            {
                                case "assets":
                                    page = names + "." + Dir + "." + page.Replace("/", ".");
                                    page = page.Replace("..", ".");

                                    string[] p = page.Split(new char[] { '.' });

                                    switch (p[p.Length - 1])
                                    {
                                        case "png":
                                        case "jpg":
                                        case "jpeg":
                                        case "eot":
                                        case "svg":
                                        case "woff":
                                        case "woff2":
                                        case "ttf":
                                        case "otf":
                                            //Debug.WriteLine("=============================");
                                            //Debug.WriteLine("Image code block here");
                                            //Debug.WriteLine("=============================");

                                            Stream s2 = assembly.GetManifestResourceStream(page);

                                            byte[] bx = new byte[8192];
                                            int bytesRead = 1;
                                            List<byte> arrayList = new List<byte>();

                                            while (bytesRead > 0)
                                            {
                                                bytesRead = s2.Read(bx, 0, bx.Length);
                                                arrayList.AddRange(new ArraySegment<byte>(bx, 0, bytesRead).Array);
                                            }

                                            byte[] b2 = arrayList.ToArray();
                                            response.ContentLength64 = b2.Length;
                                            Stream st2 = response.OutputStream;
                                            st2.Write(b2, 0, b2.Length);
                                            break;

                                        default:
                                            //Debug.WriteLine("=============================");
                                            //Debug.WriteLine("Script Request Here.............");
                                            //Debug.WriteLine(page);
                                            //Debug.WriteLine("=============================");

                                            Stream s1 = assembly.GetManifestResourceStream(page);
                                            StreamReader r1 = new StreamReader(s1);
                                            string ss1 = r1.ReadToEnd();

                                            byte[] b1 = Encoding.UTF8.GetBytes(ss1);
                                            response.ContentLength64 = b1.Length;
                                            Stream st1 = response.OutputStream;
                                            st1.Write(b1, 0, b1.Length);
                                            break;
                                    }


                                    break;

                                case "/":
                                    page = names + "." + Dir + page.Replace("/", ".");
                                    page = page.Replace("..", ".");

                                    Stream stream = assembly.GetManifestResourceStream(page);
                                    StreamReader reader = new StreamReader(stream);
                                    string msg = reader.ReadToEnd();

                                    //msg.Replace("{{PORTAL}}", "http://127.0.0.1:" + Config.port);
                                    //if (page == "XFServerApp.Assets.WebApp.Templates..js")
                                    //{
                                    //    Debug.WriteLine("=============================");
                                    //    Debug.WriteLine(msg);
                                    //    Debug.WriteLine("=============================");
                                    //}

                                    byte[] buffer = Encoding.UTF8.GetBytes(msg);
                                    response.ContentLength64 = buffer.Length;
                                    Stream st = response.OutputStream;
                                    st.Write(buffer, 0, buffer.Length);
                                    break;

                                default:
                                    page = names + @"." + Dir + @".index.html";
                                    page = page.Replace("..", ".");

                                    Stream s = assembly.GetManifestResourceStream(page);
                                    var r = new StreamReader(s);

                                    string msgx = r.ReadToEnd();
                                    //msgx.Replace("{{PORTAL}}", "http://127.0.0.1:" + Config.port);

                                    byte[] bufferx = Encoding.UTF8.GetBytes(msgx);

                                    response.ContentLength64 = bufferx.Length;
                                    Stream stx = response.OutputStream;
                                    stx.Write(bufferx, 0, bufferx.Length);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        page = names + @"." + Dir + @".index.html";
                        page = page.Replace("..", ".");

                        Stream s = assembly.GetManifestResourceStream(page);
                        var r = new StreamReader(s);
                        string msgx = r.ReadToEnd();
                        byte[] bufferx = Encoding.UTF8.GetBytes(msgx);

                        response.ContentLength64 = bufferx.Length;
                        Stream stx = response.OutputStream;
                        stx.Write(bufferx, 0, bufferx.Length);
                    }
                }
                catch (Exception e)
                {
                    //Debug.WriteLine("================");
                    //Debug.WriteLine("Catch Error");
                    //Debug.WriteLine(e.Message);
                    //Debug.WriteLine(page);
                    //Debug.WriteLine("=============");

                    string n = "console.log('" + e.Message + @"')";
                    byte[] b = Encoding.UTF8.GetBytes(n);

                    response.ContentLength64 = b.Length;
                    Stream ns = response.OutputStream;
                    ns.Write(b, 0, b.Length);
                }

                context.Response.Close();
            }
        }

        public static void Stop()
        {
            server.Stop();
        }

        public static void Reload()
        {
            Stop();
            Start();
        }
    }
}
