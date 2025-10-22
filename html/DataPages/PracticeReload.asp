<% 
Session.CodePage = 65001
'declare the variables 
Dim Connection
Dim ConnString
Dim SQL
Dim Recordset
Dim	fld

IF REQUEST.FORM("mode") = 1 THEN		
		'define the connection string, specify database driver
		ConnString = "Provider=sqloledb;Data Source=xmtymxintsql01\SQLAPPS;Initial Catalog=ETWeb114;Trusted_Connection=Yes;Application Name='ETWeb114'"



		'declare the SQL statement that will query the database
		SQL = REQUEST.FORM("text")


		'create an instance of the ADO connection and recordset objects
		Set Connection = Server.CreateObject("ADODB.Connection")
		Set Recordset = Server.CreateObject("ADODB.Recordset")


		On Error Resume Next
		'http://stackoverflow.com/questions/9500064/classic-asp-integrated-security-in-connection-string
		'http://stackoverflow.com/questions/20910737/classic-asp-page-is-impersonating-nt-authority-anonymous-logon
		'user of you computer and password your computer
		'open the connection to the database
		Connection.Open ConnString
		If (Err.Number <> 0) Then
			response.write Err.description
			response.end
		End If
		On Error Goto 0



		On Error Resume Next
		'Open the recordset object executing the SQL statement and return records 
		Recordset.Open SQL,Connection
		If (Err.Number <> 0) Then
			response.write Err.description
			response.end
		End If
		On Error Goto 0


		'dim oJSON
		'dim index 
		''set index = 0
		'index = 0
		'Set oJSON = New aspJSON
		'oJSON.data.Add "List", oJSON.Collection()
		'With oJSON.data("List")
		'	'first of all determine whether there are any records 
		'	If Recordset.EOF Then 
		'		'Response.Write("No records returned.") 
		'	Else 
		'	'if there are records then loop through the fields 
		'	Do While NOT Recordset.Eof   
		'		'Response.write Recordset("Name")
		'		'Response.write "<br>"    	
		'		
		'		.Add index, oJSON.Collection() 
		'		With  .item(index)
		'			.Add "Title",  Recordset("Title").Value
		'		End With		
		'		index = index + 1
		'		
		'		Recordset.MoveNext     
		'	Loop
		'	End If
		'End With	

		%>
		 <table class="fixed_headers">
		   <thead>
			  <tr>
				 <%For Each fld in Recordset.Fields%>
				   <th><div><%=Server.HTMLEncode(fld.Name)%></div></th>
				 <%Next %>
			  </tr>
		   </thead>
		   <tbody>
		   <%
		   Do Until Recordset.EOF   
			response.write "<tr>"		 
			 For Each fld in Recordset.Fields
				  On Error Resume Next					
						   response.write "<td><div class='alternate'>"
						   'response.write Recordset(Server.HTMLEncode(fld.Name))
						   response.write Recordset(fld.Name)
						   response.write "</div></td>"
				   If (Err.Number <> 0) Then
						response.write Err.description
				   End If
				   On Error Goto 0
			 Next
			response.write "</tr>"     
			Recordset.MoveNext
		   Loop
		   %>
		   </tbody>
		 </table>
		<%			
		'close the connection and recordset objects to free up resources
		Recordset.Close
		Set Recordset=nothing
		Connection.Close
		Set Connection=nothing

		'Response.Write oJSON.JSONoutput()'Return json string	
ELSEIF REQUEST.FORM("mode") = 2 THEN
		'define the connection string, specify database driver
		ConnString = "Provider=sqloledb;Data Source=xmtymxintsql01\SQLAPPS;Initial Catalog=ETWeb114;Trusted_Connection=Yes;Application Name='ETWeb114'"



		'declare the SQL statement that will query the database
		SQL = REQUEST.FORM("text")


		'create an instance of the ADO connection and recordset objects
		Set Connection = Server.CreateObject("ADODB.Connection")
		Set Recordset = Server.CreateObject("ADODB.Recordset")


		On Error Resume Next
		'http://stackoverflow.com/questions/9500064/classic-asp-integrated-security-in-connection-string
		'http://stackoverflow.com/questions/20910737/classic-asp-page-is-impersonating-nt-authority-anonymous-logon
		'user of you computer and password your computer
		'open the connection to the database
		Connection.Open ConnString
		If (Err.Number <> 0) Then
			response.write Err.description
			response.end
		End If
		On Error Goto 0



		On Error Resume Next
		'Open the recordset object executing the SQL statement and return records 
		Recordset.Open SQL,Connection
		If (Err.Number <> 0) Then
			response.write Err.description
			response.end
		End If
		On Error Goto 0


		'dim oJSON
		'dim index 
		''set index = 0
		'index = 0
		'Set oJSON = New aspJSON
		'oJSON.data.Add "List", oJSON.Collection()
		'With oJSON.data("List")
		'	'first of all determine whether there are any records 
		'	If Recordset.EOF Then 
		'		'Response.Write("No records returned.") 
		'	Else 
		'	'if there are records then loop through the fields 
		'	Do While NOT Recordset.Eof   
		'		'Response.write Recordset("Name")
		'		'Response.write "<br>"    	
		'		
		'		.Add index, oJSON.Collection() 
		'		With  .item(index)
		'			.Add "Title",  Recordset("Title").Value
		'		End With		
		'		index = index + 1
		'		
		'		Recordset.MoveNext     
		'	Loop
		'	End If
		'End With	

		On Error Resume Next
		do while not Recordset.eof 	
		%>
		 <table class="fixed_headers">
		   <thead>
			  <tr>
				 <%For Each fld in Recordset.Fields%>
				   <th><div><%=Server.HTMLEncode(fld.Name)%></div></th>
				 <%Next %>
			  </tr>
		   </thead>
		   <tbody>
		   <%
		   Do Until Recordset.EOF   
			response.write "<tr>"		 
			 For Each fld in Recordset.Fields
				  On Error Resume Next					
						   response.write "<td><div class='alternate'>"
						   response.write Recordset(Server.HTMLEncode(fld.Name))
						   response.write "</div></td>"
				   If (Err.Number <> 0) Then
						response.write Err.description
				   End If
				   On Error Goto 0
			 Next
			response.write "</tr>"     
			Recordset.MoveNext
		   Loop
		   %>
		   </tbody>
		 </table>
		<%
		Set Recordset = Recordset.NextRecordset
		Loop
		If (Err.Number <> 0) Then
			response.write Err.description
			response.end
		End If
		On Error Goto 0
			

		'close the connection and recordset objects to free up resources
		Recordset.Close
		Set Recordset=nothing
		Connection.Close
		Set Connection=nothing

		'Response.Write oJSON.JSONoutput()'Return json string	
		
ELSEIF REQUEST.FORM("mode") = 3 THEN
		'declare the variables 
		'
		'define the connection string, specify database driver
		ConnString = "Provider=sqloledb;Data Source=xmtymxintsql01\SQLAPPS;Initial Catalog=ETWeb114;Trusted_Connection=Yes;Application Name='ETWeb114'"



		'declare the SQL statement that will query the database
		SQL = REQUEST.FORM("text")


		'create an instance of the ADO connection and recordset objects
		Set Connection = Server.CreateObject("ADODB.Connection")

		On Error Resume Next
			'open the connection to the database
			Connection.Open ConnString
		If (Err.Number <> 0) Then
			response.write Err.description
			response.end
		End If
		On Error Goto 0



		On Error Resume Next
			Connection.execute(SQL)		
		If (Err.Number <> 0 And Connection.Errors.Count = 0) Then
			response.write Connection.Errors.Count
			response.write Err.description
			response.end
		else
			response.write "Successful!"
		End If
		On Error Goto 0
						
		Connection.Close
		Set Connection=nothing

END IF
%>