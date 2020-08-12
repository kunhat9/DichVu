using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace CORE.Base
{
    public abstract class BusinessObject
    {
        #region Data Member

        List<ValidationResult> validationResult;

        #endregion

        #region Properties

        public bool IsValid
        {
            get
            {
                validationResult = new List<ValidationResult>();
                return Validator.TryValidateObject(this, new ValidationContext(this), validationResult, true);
            }
        }

        public string ValidationResult
        {
            get
            {
                return string.Join(",", validationResult
                    .Select(vr => string.Join(",", vr.MemberNames) + ": " + vr.ErrorMessage + ";"));
            }
        }

        public string KeyName
        {
            get
            {
                return GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(x => x.GetCustomAttribute(typeof(PrimaryKey), true) != null)
                    .Select(x => x.Name)
                    .FirstOrDefault();
            }
        }

        public IEnumerable<string> KeyNames
        {
            get
            {
                return GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(x => x.GetCustomAttribute(typeof(PrimaryKey), true) != null)
                    .Select(x => x.Name);
            }
        }

        public object this[string name]
        {
            get
            {
                return GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(x => x.Name == name)
                    .Select(x => x.GetValue(this, null))
                    .FirstOrDefault();
            }
            set
            {
                if (GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(x => x.Name == name).Count() > 0)
                {
                    GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .Where(x => x.Name == name)
                        .FirstOrDefault().SetValue(this, value, null);
                }
            }
        }

        #endregion

        #region Constructor

        protected BusinessObject()
        {
            InitializeDefaultValues(this);
        }

        #endregion

        #region Protected Methods

        protected void InitializeDefaultValues(object obj)
        {
            var props = from prop in obj.GetType().GetProperties()
                        let attrs = prop.GetCustomAttributes(typeof(Default), false)
                        where attrs.Any()
                        select new { Property = prop, Attr = ((Default)attrs.First()) };
            foreach (var pair in props)
            {
                object value = !pair.Attr.IsConstructorCall && pair.Attr.Values.Length > 0
                                ? pair.Attr.Values[0]
                                : Activator.CreateInstance(pair.Property.PropertyType, pair.Attr.Values);
                pair.Property.SetValue(obj, value, null);
            }
        }

        #endregion

        public string ToStringXml()
        {
            string content = "";
            try
            {
                foreach (var p in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    content += "<" + p.Name + ">" + (p.GetValue(this, null) ?? "") + "</" + p.Name + ">";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return content;
        }

        public string ToStringXml(BusinessObject oldObject)
        {
            string content = "";
            try
            {
                if (GetType() == oldObject.GetType())
                {
                    foreach (var p in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        //Is Primary Key Or Value diffirent
                        if (p.GetCustomAttribute(typeof(PrimaryKey), true) != null
                            || p.GetValue(this, null) != p.GetValue(oldObject, null))
                        {
                            content += "<" + p.Name + ">" + (p.GetValue(this, null) ?? "") + "</" + p.Name + ">";
                        }
                    }
                }
                else
                {
                    foreach (var p in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        content += "<" + p.Name + ">" + (p.GetValue(this, null) ?? "") + "</" + p.Name + ">";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return content;
        }
    }
}
