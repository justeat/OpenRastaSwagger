<%@ Page Language="C#" %>
<%@ Import Namespace="System.Runtime.Serialization.Json" %>
<%@ Import Namespace="OpenRasta.Configuration.MetaModel" %>
<%@ Import Namespace="OpenRastaSwagger" %>
<script runat="server" language="C#">

    private void Page_Load(object sender, System.EventArgs e)
    {
        var swagger = new SwaggerDiscoverer();

        var group = Request.QueryString.ToString();
            
        if (!string.IsNullOrEmpty(group))
        {
            ToJson(swagger.GetResouceDetails(group));
            return;
        }

        ToJson(swagger.GetResourceList(x => string.Format("{0}?{1}#!/{1}", Request.ServerVariables["URL"], x.Path)));
    }

    public void ToJson<T>(T obj)
    {
        var serializer = new DataContractJsonSerializer(typeof (T));

        Response.Clear();
        Response.ContentType = "application/json";
        serializer.WriteObject(Response.OutputStream, obj);
        Response.End();
    }


</script>
