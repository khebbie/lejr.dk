<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<p>Velkommen til lejr.dk på mobilen</p>
<p>Vælg en lejr nedenfor</p>
     <ul data-role="listview" data-inset="true" data-theme="c" data-dividertheme="b"> 
            <li data-role="list-divider">Lejre</li> 
            <li><a href="/Lejr/Putte">Puttelejr</a></li> 
            <li><a href="/Lejr/Boerne">Børnelejr</a></li> 
            <li><a href="/Lejr/Junior">Juniorlejr</a></li> 
            <li><a href="/Lejr/Teen">Teenlejr</a></li> 
        </ul> 
</asp:Content>
