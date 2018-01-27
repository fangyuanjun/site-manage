using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Blogs.UI.Manage
{
    public static class CustomInputExtensions
    {
        public static MvcHtmlString InputForDate<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
         Expression<Func<TModel, TProperty>> expression)
        {

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("  <label class=\"col-sm-2 control-label\" for=\"" + metadata.PropertyName + "\">" + metadata.GetDisplayName() + "</label>");
            sb.AppendLine("  <div class=\"col-sm-10 input-group\" style=\"padding-left:15px; padding-right:15px\">");
            sb.AppendLine("     {input}");
            sb.AppendLine("      <span class=\"input-group-addon\"><span class=\"glyphicon glyphicon-calendar\"></span></span>");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            //如果model值不为，null，进行赋值。
            string value = "";
            if (metadata.Model != null)
            {
                value = "value=\"" + metadata.Model.ToString() + "\"";
            }

            string input = " <input type=\"text\" class=\"form-control {validate:{required:true,messages:{required:'请输入" + metadata.GetDisplayName() + "'}}}\" id=\"" + metadata.PropertyName + "\" name=\"" + metadata.PropertyName + "\" " + value + " onfocus=\"WdatePicker({ dateFmt: 'yyyy/MM/dd HH:mm:ss'})\" >";

            string html = sb.ToString().Replace("{input}", input);

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString InputForSelect<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression)
        {

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("  <label class=\"col-sm-2 control-label\" for=\"" + metadata.PropertyName + "\">" + metadata.GetDisplayName() + "</label>");
            sb.AppendLine("  <div class=\"col-sm-10\">");
            sb.AppendLine("     {input}");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            string input = " <select type=\"text\" class=\"form-control\" id=\"" + metadata.PropertyName + "\" name=\"" + metadata.PropertyName + "\"  ></select>";

            string html = sb.ToString().Replace("{input}", input);

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString InputForCheckbox<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
       Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("  <label class=\"col-sm-2 control-label\" for=\"" + metadata.PropertyName + "\">" + metadata.GetDisplayName() + "</label>");
            sb.AppendLine("  <div class=\"col-sm-10\" style=\"padding-top:6px\">");
            sb.AppendLine("     {input}");
            sb.AppendLine("     {input2}");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            //如果model值不为，null，进行赋值。
            string chk = "";
            string value = "";
            if (metadata.Model != null)
            {
                value = metadata.Model.ToString();
                if (value == "1" || value.ToLower() == "true")
                {
                    chk = "checked";
                }

                value = "value=\"" + metadata.Model.ToString() + "\"";
            }

            string input = " <input type=\"checkbox\" id=\"" + metadata.PropertyName + "\"  " + chk + ">";
            string input2 = "<input type=\"hidden\" id=\"_" + metadata.PropertyName + "\" name=\"" + metadata.PropertyName + "\"  " + value + ">";
            string html = sb.ToString().Replace("{input}", input).Replace("{input2}", input2);

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString InputForSwitch<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("  <label class=\"col-sm-2 control-label\" for=\"" + metadata.PropertyName + "\">" + metadata.GetDisplayName() + "</label>");
            sb.AppendLine("  <div class=\"col-sm-10 switch\" >");
            sb.AppendLine("     {input}");
            sb.AppendLine("     {input2}");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            //如果model值不为，null，进行赋值。
            string chk = "";
            string value = "";
            if (metadata.Model != null)
            {
                value = metadata.Model.ToString();
                if (value == "1" || value.ToLower() == "true")
                {
                    chk = "checked";
                }

                value = "value=\"" + metadata.Model.ToString() + "\"";
            }

            string input = " <input type=\"checkbox\" id=\"" + metadata.PropertyName + "\"  " + chk + ">";
            string input2 = "<input type=\"hidden\" id=\"_" + metadata.PropertyName + "\" name=\"" + metadata.PropertyName + "\"  " + value + ">";
            string html = sb.ToString().Replace("{input}", input).Replace("{input2}", input2);

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString InputForPassword<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("  <label class=\"col-sm-2 control-label\" for=\"" + metadata.PropertyName + "\">" + metadata.GetDisplayName() + "</label>");
            sb.AppendLine("  <div class=\"col-sm-10\">");
            sb.AppendLine("     {input}");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            //如果model值不为，null，进行赋值。
            string value = "";
            if (metadata.Model != null)
            {
                value = metadata.Model.ToString();
            }

            string input = " <input type=\"password\" id=\"" + metadata.PropertyName + "\" name=\"" + metadata.PropertyName + "\"  " + value + ">";

            string html = sb.ToString().Replace("{input}", input);

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString InputForRadio<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("  <label class=\"col-sm-2 control-label\" for=\"" + metadata.PropertyName + "\">" + metadata.GetDisplayName() + "</label>");
            sb.AppendLine("  <div class=\"col-sm-10\" style=\"padding-top:6px\">");
            sb.AppendLine("     {input}");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            //如果model值不为，null，进行赋值。
            string value = "";
            if (metadata.Model != null)
            {
                value = metadata.Model.ToString();
            }

            string input = " <input type=\"radio\"  name=\"" + metadata.PropertyName + "\"  " + value + ">";

            string html = sb.ToString().Replace("{input}", input);

            return new MvcHtmlString(html);
        }


        public static MvcHtmlString TextBoxForBoot<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
          Expression<Func<TModel, TProperty>> expression)
        {
            /*
             *  @Html.TextBoxForBoot(model => model.linkName)
             *  
             * <div class="form-group">
                <label class="col-sm-2 control-label" for="linkUrl">链接地址</label>
                <div class="col-sm-10">
                    <input class="form-control {validate:{required:true,messages:{required:'请输入姓名', minlength:'请至少输入两个字符'}}}" id="linkUrl" name="linkUrl" type="text">
                    <span class="field-validation-valid" data-valmsg-for="linkUrl" data-valmsg-replace="true"></span>
                </div>
            </div>
             * */
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("  <label class=\"col-sm-2 control-label\" for=\"" + metadata.PropertyName + "\">" + metadata.GetDisplayName() + "</label>");
            sb.AppendLine("  <div class=\"col-sm-10\">");
            sb.AppendLine("     {input}");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            // class="{validate:{required: true, minlength: 2, messages:{required:'请输入姓名', minlength:'请至少输入两个字符'}}}"  
            string style = "form-control";
            string rules = "";
            string messages = "";
            if (metadata.IsRequired)
            {
                rules += "required:true,";
                messages += "required:'请输入" + metadata.GetDisplayName() + "',";
            }

            var min = metadata.ContainerType.GetProperty(metadata.PropertyName).GetCustomAttribute(typeof(MinLengthAttribute));
            if (min != null)
            {
                rules += "minlength:" + ((MinLengthAttribute)min).Length + ",";
                messages += "minlength:'" + ((MinLengthAttribute)min).ErrorMessage + "',";
            }

            var max = metadata.ContainerType.GetProperty(metadata.PropertyName).GetCustomAttribute(typeof(MaxLengthAttribute));
            if (max != null)
            {
                rules += "maxlength:" + ((MaxLengthAttribute)max).Length + ",";
                messages += "maxlength:'" + ((MaxLengthAttribute)max).ErrorMessage + "',";
            }

            var email = metadata.ContainerType.GetProperty(metadata.PropertyName).GetCustomAttribute(typeof(EmailAddressAttribute));
            if (email != null)
            {
                rules += "email:true,";
                messages += "email:'" + ((EmailAddressAttribute)email).ErrorMessage + "',";
            }


            if (!String.IsNullOrEmpty(rules))
            {
                style += " {validate:{" + rules.TrimEnd(',') + ",messages:{" + messages.TrimEnd(',') + "}}}";
            }

            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.AddCssClass(style);
            tagBuilder.MergeAttribute("name", metadata.PropertyName);
            tagBuilder.GenerateId(metadata.PropertyName);


            //如果model值不为，null，进行赋值。
            if (metadata.Model != null)
            {
                tagBuilder.MergeAttribute("value", metadata.Model.ToString());
            }

            if (min != null)
            {
                tagBuilder.MergeAttribute("minlength", ((MinLengthAttribute)min).Length + "");
            }

            if (max != null)
            {
                tagBuilder.MergeAttribute("maxlength", ((MaxLengthAttribute)max).Length + "");
            }

            if (email != null)
            {
                tagBuilder.MergeAttribute("type", "email");
            }
            else
            {
                //if(metadata.ModelType==typeof(DateTime))
                //{
                //    tagBuilder.MergeAttribute("type", "datetime-local");
                //}
                //else
                //{
                tagBuilder.MergeAttribute("type", "text");
                //}
            }


            string html = sb.ToString().Replace("{input}", tagBuilder.ToString().Replace("</input>", ""));

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString TextBoxForBootReadonly<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression)
        {

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("  <label class=\"col-sm-2 control-label\" for=\"" + metadata.PropertyName + "\">" + metadata.GetDisplayName() + "</label>");
            sb.AppendLine("  <div class=\"col-sm-10\">");
            sb.AppendLine("     {input}");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            //如果model值不为，null，进行赋值。
            string value = "";
            if (metadata.Model != null)
            {
                value = "value=\"" + metadata.Model.ToString() + "\"";
            }

            string input = " <input type=\"text\" class=\"form-control\" id=\"" + metadata.PropertyName + "\" name=\"" + metadata.PropertyName + "\" " + value + " readonly >";

            string html = sb.ToString().Replace("{input}", input);

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString InputForTextareaReadonly<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
       Expression<Func<TModel, TProperty>> expression)
        {

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("  <label class=\"col-sm-2 control-label\" for=\"" + metadata.PropertyName + "\">" + metadata.GetDisplayName() + "</label>");
            sb.AppendLine("  <div class=\"col-sm-10\">");
            sb.AppendLine("     {input}");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            //如果model值不为，null，进行赋值。
            string value = "";
            if (metadata.Model != null)
            {
                value = metadata.Model.ToString();
            }

            string input = " <textarea  type=\"text\" cols=\"20\"  rows=\"3\" class=\"form-control\" id=\"" + metadata.PropertyName + "\" name=\"" + metadata.PropertyName + "\"  readonly>" + value + "</textarea>";

            string html = sb.ToString().Replace("{input}", input);

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString InputForTextarea<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression)
        {

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendLine("  <label class=\"col-sm-2 control-label\" for=\"" + metadata.PropertyName + "\">" + metadata.GetDisplayName() + "</label>");
            sb.AppendLine("  <div class=\"col-sm-10\">");
            sb.AppendLine("     {input}");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            // class="{validate:{required: true, minlength: 2, messages:{required:'请输入姓名', minlength:'请至少输入两个字符'}}}"  
            string style = "form-control";
            string rules = "";
            string messages = "";
            if (metadata.IsRequired)
            {
                rules += "required:true,";
                messages += "required:'请输入" + metadata.GetDisplayName() + "',";
            }

            var min = metadata.ContainerType.GetProperty(metadata.PropertyName).GetCustomAttribute(typeof(MinLengthAttribute));
            if (min != null)
            {
                rules += "minlength:" + ((MinLengthAttribute)min).Length + ",";
                messages += "minlength:'" + ((MinLengthAttribute)min).ErrorMessage + "',";
            }

            var max = metadata.ContainerType.GetProperty(metadata.PropertyName).GetCustomAttribute(typeof(MaxLengthAttribute));
            if (max != null)
            {
                rules += "maxlength:" + ((MaxLengthAttribute)max).Length + ",";
                messages += "maxlength:'" + ((MaxLengthAttribute)max).ErrorMessage + "',";
            }

            var email = metadata.ContainerType.GetProperty(metadata.PropertyName).GetCustomAttribute(typeof(EmailAddressAttribute));
            if (email != null)
            {
                rules += "email:true,";
                messages += "email:'" + ((EmailAddressAttribute)email).ErrorMessage + "',";
            }


            if (!String.IsNullOrEmpty(rules))
            {
                style += " {validate:{" + rules.TrimEnd(',') + ",messages:{" + messages.TrimEnd(',') + "}}}";
            }

            //如果model值不为，null，进行赋值。
            string value = "";
            if (metadata.Model != null)
            {
                value = metadata.Model.ToString();
            }

            string input = " <textarea  type=\"text\" cols=\"20\"  rows=\"3\" class=\"  " + style + "\" id=\"" + metadata.PropertyName + "\" name=\"" + metadata.PropertyName + "\"  >" + value + "</textarea>";

            string html = sb.ToString().Replace("{input}", input);

            return new MvcHtmlString(html);
        }
    }
}