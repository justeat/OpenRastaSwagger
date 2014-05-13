<%@ Page Language="C#" %>
<%@ Import Namespace="System.Runtime.Serialization.Json" %>
<%@ Import Namespace="OpenRastaSwagger" %>
<%@ Import Namespace="OpenRastaSwagger.ContractJsonGeneration" %>
<script runat="server" language="C#">

    private void Page_Load(object sender, System.EventArgs e)
    {
        var contract = new ContractDiscoverer();

        ToJson(contract.GetContract());
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
