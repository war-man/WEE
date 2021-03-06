﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WEE_API.Common
{
    public static class CommonFunction
    {
        public static Expression<Func<T, object>> CreateNewStatement<T>(string fields)
        {
            // input parameter "o"
            var xParameter = Expression.Parameter(typeof(T), "o");

            // new statement "new T()"
            var xNew = Expression.New(typeof(T));

            // create initializers
            var bindings = fields.Split(',').Select(o => o.Trim())
                .Select(o =>
                    {
                        var label = o.Split('|')[0];
                        var value = o.Split('|')[1];
                        // property "Field1"
                        var mi = typeof(T).GetProperty(label);

                        // original value "o.Field1"
                        var xOriginal = Expression.Property(xParameter, value);

                        // set value "Field1 = o.Field1"
                        return Expression.Bind(mi, xOriginal);
                    }
                );

            // initialization "new T { Field1 = o.Field1, Field2 = o.Field2 }"
            var xInit = Expression.MemberInit(xNew, bindings);

            // expression "o => new T { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<T, object>>(xInit, xParameter);

            // compile to Func<Data, Data>
            return lambda;
        }
        public static fileJSON GenImageJSON(string filename)
        {
            var fileTmp = new fileJSON()
            {
                files = new FileTable()
                {
                    files = new Dictionary<string, FileInfo>()
                    {
                        {filename,new FileInfo() {id = filename,web_path = filename} }
                    }
                },
                upload = new FileUpload() { id = filename }

            };
            return fileTmp;
        }


        public static FileTable GenListImageJSON(List<string> filename)
        {
            var files = new FileTable { files = new Dictionary<string, FileInfo>() };
            foreach (var name in filename)
            {
                files.files.Add(name, new FileInfo() { id = name, web_path = name });
            }
            return files;
        }
    }

    public class fileJSON
    {
        public FileTable files { get; set; }
        public FileUpload upload { get; set; }
    }
    public class FileUpload
    {
        public string id { get; set; }
    }
    public class FileTable
    {
        public Dictionary<string, FileInfo> files { get; set; }
    }

    public class FileInfo
    {
        public string id { get; set; }
        public string filename { get; set; }
        public string filesize { get; set; }
        public string web_path { get; set; }
        public string system_path { get; set; }
    }

}