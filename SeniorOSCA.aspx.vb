Imports System.Activities.Expressions
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Web.Services
Imports MySql.Data.MySqlClient
Imports QRCoder
Partial Class SeniorOSCA
    Inherits System.Web.UI.Page
    Dim ConnString As String
    Dim clsMaster As New cls_master
    Dim requestID As String

    Protected Sub SeniorOSCA_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not Request.QueryString("ID") = Nothing Then
            requestID = clsMaster.Decrypt(Request.QueryString("ID").Replace(" ", "+"))

        End If

        ConnString = ConfigurationManager.ConnectionStrings("conn1").ConnectionString
    End Sub

    Private Sub SeniorOSCA_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If clsMaster.checkUser = 1 Or clsMaster.checkUser = 2 Then
                header_add.Visible = True
                btn_submit.Visible = True
            End If

            Try
                If Not requestID = Nothing Then
                    div_list.Visible = False
                    div_details.Attributes.Remove("hidden")
                    showDetails()
                    EnableFieldsExceptNamesAndBDate()
                    'Else

                    '    DisableAddNewFields()
                End If
            Catch ex As Exception
                ClientScript.RegisterStartupScript(Me.[GetType](), "systemMsg", "systemMsg(0, '" & ex.Message.Replace("'", "|") & "');", True)
            End Try
        End If
    End Sub


    <WebMethod()>
    Public Shared Function CheckSeniorCitizenExists(refID As String) As Dictionary(Of String, Object)
        Dim result As New Dictionary(Of String, Object)
        Dim connString As String = ConfigurationManager.ConnectionStrings("conn1").ConnectionString
        Try
            Dim count As Integer = 0
            Using conn As New MySqlConnection(connString)
                conn.Open()
                Dim existsQuery As String = "SELECT COUNT(*) FROM osca_seniorcitizen_tbl WHERE OldRefID = @RefID"
                Using cmd As New MySqlCommand(existsQuery, conn)
                    cmd.Parameters.AddWithValue("@RefID", refID)
                    count = Convert.ToInt32(cmd.ExecuteScalar())
                End Using
            End Using

            result.Add("exists", count > 0)
            result.Add("success", True)
        Catch ex As Exception
            result.Add("exists", False)
            result.Add("success", False)
            result.Add("error", ex.Message)
        End Try
        Return result
    End Function

    Private Sub DisableAddNewFields()
        txt_ownerName.Disabled = True
        txt_ownerID.Disabled = True
        dl_type.Disabled = True
        dl_dswdPentioner.Disabled = True
        txt_ncscRRN.Disabled = True
        dl_notarizedFormWithID.Disabled = True
        dl_fullBodyID.Disabled = True
        Dim priReq = TryCast(Me.FindControl("dl_priReq"), HtmlSelect)
        If priReq IsNot Nothing Then priReq.Disabled = True
        Dim secReq = TryCast(Me.FindControl("dl_secReq"), HtmlSelect)
        If secReq IsNot Nothing Then secReq.Disabled = True
        txt_FName.Disabled = True
        txt_MName.Disabled = True
        txt_LName.Disabled = True
        dl_suffix.Disabled = True
        dl_sex.Disabled = True
        dl_civilStatus.Disabled = True
        'txt_BDate.Disabled = False
        txt_BPLace.Disabled = True
        txt_Citizenship.Disabled = True
        txt_Height.Disabled = True
        txt_Weight.Disabled = True
        dl_lgbtq.Disabled = True
        txt_contactNo.Disabled = True
        dl_brgycity.Disabled = True
        txt_Address.Disabled = True
        txt_remarks.Disabled = True
        select_status.Disabled = True
        btn_submit.Disabled = True
    End Sub

    Private Sub EnableFieldsExceptNamesAndBDate()
        ' Enable all fields
        txt_ownerName.Disabled = False
        txt_ownerID.Disabled = False
        dl_type.Disabled = False
        dl_dswdPentioner.Disabled = False
        txt_ncscRRN.Disabled = False
        dl_notarizedFormWithID.Disabled = False
        dl_fullBodyID.Disabled = False
        Dim priReq = TryCast(Me.FindControl("dl_priReq"), HtmlSelect)
        If priReq IsNot Nothing Then priReq.Disabled = False
        Dim secReq = TryCast(Me.FindControl("dl_secReq"), HtmlSelect)
        If secReq IsNot Nothing Then secReq.Disabled = False
        dl_suffix.Disabled = False
        dl_sex.Disabled = False
        dl_civilStatus.Disabled = False
        txt_BPLace.Disabled = False
        txt_Citizenship.Disabled = False
        txt_Height.Disabled = False
        txt_Weight.Disabled = False
        dl_lgbtq.Disabled = False
        txt_contactNo.Disabled = False
        dl_brgycity.Disabled = False
        txt_Address.Disabled = False
        txt_remarks.Disabled = False
        select_status.Disabled = False
        btn_submit.Disabled = False
        ' Disable only FName, MName, LName, BDate
        txt_FName.Disabled = True
        txt_MName.Disabled = True
        txt_LName.Disabled = True
        'txt_BDate.Disabled = False
    End Sub

    <WebMethod>
    Public Shared Function ViewCitizenSearch(keyword As String, profileType As String) As String
        Dim clsMaster As New cls_master
        Dim htmlStr As New StringBuilder

        Try
            With clsMaster
                For Each row In .dynamicTbl(
                    "CitizenProfiles",
                    "(maincitizenprofile_tbl.fName LIKE '%" & keyword.Trim & "%' OR maincitizenprofile_tbl.lName LIKE '%" & keyword.Trim & "%' OR maincitizenprofile_tbl.mName LIKE '%" & keyword.Trim & "%') " &
                    "AND TIMESTAMPDIFF(YEAR, maincitizenprofile_tbl.bdate, CURDATE()) >= 60 " &
                    "AND NOT EXISTS (SELECT 1 FROM osca_seniorcitizen_tbl WHERE osca_seniorcitizen_tbl.OldRefID = maincitizenprofile_tbl.RefID)",
                    "sp_dynamicDT"
                ).AsEnumerable
                    Dim profile As New Dictionary(Of String, String) From {
                    {"RefID", row.Item("RefID").ToString()},
                    {"fName", row.Item("fName").ToString().Replace("'", "`")},
                    {"mName", row.Item("mName").ToString()},
                    {"lName", row.Item("lName").ToString()},
                    {"suffix", row.Item("suffix").ToString()},
                    {"height", row.Item("height").ToString()},
                    {"weight", row.Item("weight").ToString()},
                    {"civilStatus", row.Item("civilStatus").ToString()},
                    {"BPlace", row.Item("BPlace").ToString()},
                    {"sex", If(row.Item("sex").ToString = "1", "FEMALE", "MALE")},
                    {"lgbtq", row.Item("lgbtq").ToString()},
                    {"bdate", row.Item("bdate").ToString()},
                    {"contactNo", row.Item("contactNo").ToString()},
                    {"address", row.Item("address").ToString().Replace("'", "`")},
                    {"4ps", row.Item("4ps").ToString()},
                    {"DSWDPensioner", row.Item("DSWDPensioner").ToString()},
                    {"ncscrrn", row.Item("ncscrrn").ToString()},
                    {"locationID", row.Item("locationID").ToString()},
                    {"status", row.Item("status").ToString()},
                    {"barangay", row.Item("Name").ToString()},
                    {"Citizenship", row.Item("Citizenship").ToString()}
                }
                    ' Convert dictionary to JSON formatf
                    Dim jsonData As String = HttpUtility.JavaScriptStringEncode(Newtonsoft.Json.JsonConvert.SerializeObject(profile))

                    htmlStr.Append("<tr>")
                    htmlStr.Append("<td>")
                    htmlStr.Append("<strong>Name: </strong>" & profile("fName") & " " & profile("mName") & " " & profile("lName") & " " & profile("suffix") & "<br/>")
                    htmlStr.Append("<strong>Sex: </strong>" & profile("sex") & "<br/>")
                    htmlStr.Append("<strong>Birthdate: </strong>" & profile("bdate") & "<br/>")
                    htmlStr.Append("<strong>Address: </strong>" & profile("address") & "<br/>")
                    htmlStr.Append("<strong>4Ps Status: </strong>" & profile("4ps") & "<br/>")
                    htmlStr.Append("<strong>Barangay: </strong>" & profile("barangay"))
                    htmlStr.Append("</td>")
                    htmlStr.Append("<td style='width: 20%; text-align: center'>")

                    ' Pass JSON to JavaScript
                    htmlStr.Append("<button type='button' class='btn btn-primary btn-sm' onclick='SelectCitizenProfile(`" & jsonData & "` )'>Select</button>")

                    htmlStr.Append("</td>")
                    htmlStr.Append("</tr>")
                Next
            End With

            Return htmlStr.ToString()
        Catch ex As Exception
            Return "Error: " + ex.Message.Replace("'", "|")
        End Try
    End Function



    <WebMethod>
    Public Shared Function genID(profileList As String, whatButton As String) As String
        Dim clsMaster2 As New cls_master
        Dim htmlStr As New StringBuilder
        Dim filterList As New List(Of String)

        With clsMaster2
            Dim moduleAccessString As String = HttpContext.Current.Session("moduleAccess").ToString()
            Dim moduleAccessArray As String() = moduleAccessString.Split(","c)
            Dim sql As String = ""

            For Each row In .deserializeJSON(profileList).AsEnumerable
                filterList.Add("'" & row.Item("col1").ToString() & "'")
            Next

            For i As Integer = 0 To moduleAccessArray.Length - 1
                Dim moduleName As String = moduleAccessArray(i).Trim()

                If moduleName = "Senior Citizen" Or whatButton = "Senior Citizen" Then
                    sql = "SELECT osca.RefID, osca.profileName, osca.address, osca.bDate, osca.locationID, osca.sex, osca.age, osca.IDnumber, osca.fileName, osca.ncscrrn " &
                           "FROM (" &
                           "  SELECT cp.RefID, " &
                           "         CONCAT_WS(' ', COALESCE(cp.fName, ''), COALESCE(cp.mName, ''), COALESCE(cp.lName, ''), COALESCE(cp.suffix, '')) AS profileName, " &
                           "         loc.Name AS address, " &
                           "         DATE_FORMAT(cp.bdate, '%M %e, %Y') AS bDate, " &
                           "         cp.locationID, " &
                           "         CASE WHEN cp.sex = 1 THEN 'FEMALE' ELSE 'MALE' END AS sex, " &
                           "         TIMESTAMPDIFF(YEAR, cp.bdate, CURDATE()) AS age, " &
                           "         CONCAT(cp.RefID, CASE WHEN TIMESTAMPDIFF(YEAR, cp.bdate, CURDATE()) BETWEEN 60 AND 100 THEN '-SN' ELSE '' END) AS IDnumber, " &
                           "         '' AS fileName, " &
                           "         cp.ncscrrn " &
                           "  FROM maincitizenprofile_tbl cp " &
                           "  INNER JOIN rpt_locations_tbl loc ON cp.locationID = loc.ID " &
                           "  WHERE TIMESTAMPDIFF(YEAR, cp.bdate, CURDATE()) BETWEEN 60 AND 100" &
                           ") AS osca " &
                           "WHERE osca.RefID IN (" & String.Join(", ", filterList) & ")"

                End If

            Next


            htmlStr.Append("<div id='printableArea'>") ' Wrapper div for printing
            For Each row In .dynamicQuery(sql).AsEnumerable

                Dim profileNameSize As Integer = row.Item("profileName").ToString.Length
                Dim bDateFont As Integer = row.Item("bDate").ToString.Length
                Dim fontSize1 As String = "12px"
                Dim bDateFontSize As String = "8.5px"

                If profileNameSize >= 41 Then
                    fontSize1 = "9px"
                ElseIf profileNameSize >= 23 Then
                    fontSize1 = "11px"
                End If

                If bDateFont >= 18 Then
                    bDateFontSize = "7px"
                End If

                Dim qrCode As String = barcodeString(row.Item("RefID").ToString())
                Dim imagePath As String = "Uploads/" & row.Item("RefID").ToString() & "/" & row.Item("fileName").ToString()

                For i As Integer = 0 To moduleAccessArray.Length - 1
                    Dim moduleName As String = moduleAccessArray(i).Trim()


                    If moduleName = "Senior Citizen" Or whatButton = "Senior Citizen" Then
                        ' FRONT ID
                        htmlStr.Append("<div style='display: flex; flex-direction: row; width: 100%; margin: 1px 0;'>")
                        htmlStr.Append("<div style='position: relative; width: 3.65in; height: 2.65in; margin-right: 1px;'>")
                        htmlStr.Append("<img src='Images/ID-SNfront.png' style='width: 100%; height: 100%'/>")

                        htmlStr.Append("<div style='position: absolute; top: 0.758in; left: 1.6in; font-family: Poppins; color: #000; font-size: 11px; text-align: center'>" & row.Item("IDnumber").ToString & "</div>")
                        htmlStr.Append("<div style='position: absolute; top: 0.94in; left: 1.87in; font-family: Poppins; color: #000; font-size: 11px; text-align: center'>" & row.Item("ncscrrn").ToString & "</div>")
                        htmlStr.Append("<div style='position: absolute; top: 1.15in; left: 1.7in; font-family: Poppins; color: #000; font-size: " & fontSize1 & "; text-align: center'>" & row.Item("profileName").ToString.ToUpper() & "</div>")
                        htmlStr.Append("<div style='position: absolute; top: 1.3in; left: 1.8in; font-family: Poppins; font-size: 10px; color: #000; line-height: 1.9; text-align: center'>" & row.Item("address").ToString & "</div>")
                        htmlStr.Append("<div style='position: absolute; top: 1.52in; left: 1.83in; font-family: Poppins; color: #000; font-size: " & bDateFontSize & "; text-align: center'>" & row.Item("bDate").ToString.ToUpper() & "</div>")
                        htmlStr.Append("<div style='position: absolute; top: 1.52in; left: 2.85in; font-family: Poppins; color: #000; font-size: 10px; text-align: center'>" & row.Item("age").ToString.ToUpper() & "</div>")
                        htmlStr.Append("<div style='position: absolute; top: 1.52in; left: 3.34in; font-family: Poppins; color: #000; font-size: 10px; text-align: center'>" & If(row.Item("sex").ToString.ToUpper() = "FEMALE", "F", "M") & "</div>")
                        htmlStr.Append("<div style='position: absolute; top: 1.69in; left: 2.03in; font-family: Poppins; font-size: 9px; color: #000; line-height: 1.9; text-align: center'>" & .dateNow.ToString("MMMM dd, yyyy hh:mm tt") & "</div>")
                        htmlStr.Append("</div>")
                        htmlStr.Append("<div style='position: absolute; top: 1.85in; left: 0.065in; width: 2.5in; height: 0.5in; background-color: transparent;'>")
                        htmlStr.Append("</div>")
                        ' BACK ID
                        htmlStr.Append("<div style='position: relative; width: 3.65in; height: 2.65in;'>")
                        htmlStr.Append("<img src='Images/ID-SNback.png' style='width: 100%; height: 100%'/>")

                        htmlStr.Append("<div style='position: absolute; top: 0.3in; right: 0; width: 60px; text-align: right;'>")
                        htmlStr.Append("<img src='" + .barcodeString("http://localhost:61633/ScanQR.aspx?ID=" & .EncryptEportal(row.Item("RefID")) & ":OSC") + "' style='width: auto; height: 57px;'/>")
                        htmlStr.Append("</div>")


                        htmlStr.Append("<div style='position: absolute; top: 0.47in; left: 2.25in;'>")

                        htmlStr.Append("</div>")
                        htmlStr.Append("</div>")
                        htmlStr.Append("</div>")


                    End If
                Next

            Next
            htmlStr.Append("</div>") 

            htmlStr.Append("<div style='position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);'>")
            htmlStr.Append("<button onclick='printIDs()' style='padding: 10px 20px; font-size: 14px; background: #28a745; color: white; border: none; cursor: pointer;'>Print All IDs</button>")
            htmlStr.Append("</div>")

            htmlStr.Append("<script>")
            htmlStr.Append("function printIDs() {")
            htmlStr.Append("var printContents = document.getElementById('printableArea').innerHTML;")
            htmlStr.Append("var originalContents = document.body.innerHTML;")
            htmlStr.Append("document.body.innerHTML = printContents;")
            htmlStr.Append("window.print();")
            htmlStr.Append("document.body.innerHTML = originalContents;")
            htmlStr.Append("location.reload();") 
            htmlStr.Append("}")
            htmlStr.Append("</script>")
        End With
        Return htmlStr.ToString
    End Function

    <WebMethod>
    Public Shared Function ViewSearch(keyword As String, whatButton As String) As String
        Dim clsMaster As New cls_master
        Dim htmlStr As New StringBuilder
        Try
            With clsMaster
                Dim tableDT As New DataTable
                Dim excludeList As New List(Of String)
                Dim sql As String = ""

                Dim moduleAccessString As String = HttpContext.Current.Session("moduleAccess").ToString()
                Dim moduleAccessArray As String() = moduleAccessString.Split(","c)

                For i As Integer = 0 To moduleAccessArray.Length - 1
                    Dim moduleName As String = moduleAccessArray(i).Trim()

                    If moduleName = "Senior Citizen" Or whatButton = "Senior Citizen" Then
                        sql = "SELECT osca.RefID, osca.profileName, osca.bDate, osca.address " &
                  "FROM (" &
                  "  SELECT osca.RefID, " &
                  "         CONCAT_WS(' ', COALESCE(osca.fName, ''), COALESCE(osca.mName, ''), COALESCE(osca.lName, ''), COALESCE(osca.suffix, '')) AS profileName, " &
                  "         DATE_FORMAT(osca.bDate, '%M %e, %Y') AS bDate, " &
                  "         loc.Name AS address " &
                  "  FROM osca_seniorcitizen_tbl osca " &
                  "  LEFT JOIN rpt_locations_tbl loc ON loc.ID = osca.locationID " &
                  "  WHERE osca.status = 1 " &
                  "    AND LOWER(CONCAT_WS(' ', COALESCE(osca.fName, ''), COALESCE(osca.mName, ''), COALESCE(osca.lName, ''), COALESCE(osca.suffix, ''))) LIKE LOWER('%" & keyword & "%') " &
                  "    AND TIMESTAMPDIFF(YEAR, osca.bDate, CURDATE()) BETWEEN 60 AND 100 " &
                  "  UNION " &
                  "  SELECT cp.RefID, " &
                  "         CONCAT_WS(' ', COALESCE(cp.fName, ''), COALESCE(cp.mName, ''), COALESCE(cp.lName, ''), COALESCE(cp.suffix, '')) AS profileName, " &
                  "         DATE_FORMAT(cp.bDate, '%M %e, %Y') AS bDate, " &
                  "         loc2.Name AS address " &
                  "  FROM maincitizenprofile_tbl cp " &
                  "  INNER JOIN rpt_locations_tbl loc2 ON loc2.ID = cp.locationID " &
                  "  WHERE LOWER(CONCAT_WS(' ', COALESCE(cp.fName, ''), COALESCE(cp.mName, ''), COALESCE(cp.lName, ''), COALESCE(cp.suffix, ''))) LIKE LOWER('%" & keyword & "%') " &
                  "    AND TIMESTAMPDIFF(YEAR, cp.bDate, CURDATE()) BETWEEN 60 AND 100 " &
                  ") AS osca " &
                  "ORDER BY osca.profileName;"
                    End If
                Next

                tableDT = .dynamicQuery(sql)

                For Each row In tableDT.AsEnumerable
                    Dim particularInfo As New List(Of String)
                    particularInfo.Add("<strong>Name: </strong>" & row.Item("profileName").ToString.ToUpper)
                    particularInfo.Add("<strong>Birthdate: </strong>" & row.Item("bDate").ToString)
                    particularInfo.Add("<strong>Address: </strong>" & row.Item("address").ToString.ToUpper)

                    htmlStr.Append("<tr>")
                    htmlStr.Append("<td>" + String.Join("<br/>", particularInfo) + "</td>")
                    htmlStr.Append("<td style='width: 20%; text-align: center'><button type='button' class='btn btn-info btn-sm btn-select-profile'" &
                          " data-refId='" & row.Item("RefID").ToString & "'" &
                          " data-bdate='" & HttpUtility.JavaScriptStringEncode(row.Item("bDate").ToString.Replace("'", "").ToUpper) & "'" &
                          " data-address='" & HttpUtility.JavaScriptStringEncode(row.Item("address").ToString.Replace("'", "").ToUpper) & "'" &
                          " data-profilename='" & HttpUtility.JavaScriptStringEncode(row.Item("profileName").ToString.Replace("'", "").ToUpper) & "'>Select</button></td>")
                    htmlStr.Append("</tr>")
                Next
            End With

            Return htmlStr.ToString
        Catch ex As Exception
            Return "Error: " + ex.Message.Replace("'", "|")
        End Try
    End Function

    ' QR CODE
    Public Shared Function barcodeString(str As String) As String
        Dim qrGenerator As New QRCodeGenerator()
        Dim QRCodeData As QRCodeData = qrGenerator.CreateQrCode(str, QRCodeGenerator.ECCLevel.Q)
        Dim QRCode As New QRCode(QRCodeData)
        Dim qrString As String

        Dim foregroundColor As Color = Color.Black
        Dim backgroundColor As Color = Color.Transparent

        Using bitmap As Bitmap = QRCode.GetGraphic(20, foregroundColor, backgroundColor, True)
            Using ms As New MemoryStream()
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                Dim byteImage As Byte() = ms.ToArray()
                qrString = "data:image/png;base64," & Convert.ToBase64String(byteImage)
            End Using
        End Using

        Return qrString
    End Function

    Private Sub showDetails()
        With clsMaster
            Dim filterList As New List(Of String)

            Dim foundRow As DataRow() = .dynamicTbl("OSCAProfile", "RefID = '" & requestID & "'").Select()

            If foundRow.Length > 0 Then
                txt_refID.Value = foundRow(0).Item("refID").ToString
                h3_title.InnerText = "Ref ID: " + foundRow(0).Item("refID").ToString
                ul_tabs.Visible = True
                'btn_inactive.Visible = True
                txt_ownerName.Value = foundRow(0).Item("fName").ToString() & " " & foundRow(0).Item("mName").ToString() & " " & foundRow(0).Item("lName").ToString()


                If Not clsMaster.checkUser = 1 Then
                    txt_FName.Disabled = True
                    txt_LName.Disabled = True
                End If

                txt_BDate.Value = foundRow(0).Item("bdate").ToString
                dl_type.Value = foundRow(0).Item("TypeID").ToString
                txt_brgycity.Value = foundRow(0).Item("locationID").ToString
                dl_brgycity.Value = foundRow(0).Item("locationID").ToString
                'txt_brgyID.Value = foundRow(0).Item("brgyID").ToString
                txt_Address.Value = foundRow(0).Item("address").ToString
                txt_FName.Value = foundRow(0).Item("fName").ToString
                txt_MName.Value = foundRow(0).Item("mName").ToString
                txt_LName.Value = foundRow(0).Item("lName").ToString
                dl_suffix.Value = foundRow(0).Item("suffix").ToString
                dl_civilStatus.Value = foundRow(0).Item("civilStatus").ToString
                dl_lgbtq.Value = foundRow(0).Item("lgbtq").ToString
                dl_dswdPentioner.Value = foundRow(0).Item("DSWDPensioner").ToString
                txt_ncscRRN.Value = foundRow(0).Item("ncscrrn").ToString
                dl_sex.Value = foundRow(0).Item("sex").ToString
                txt_BPLace.Value = foundRow(0).Item("BPlace").ToString
                txt_Citizenship.Value = foundRow(0).Item("Citizenship").ToString.ToUpper
                txt_Height.Value = foundRow(0).Item("Height").ToString().Replace(",", ".")
                txt_Weight.Value = foundRow(0).Item("Weight").ToString().Replace(",", ".")
                txt_contactNo.Value = foundRow(0).Item("contactNo").ToString
                txt_remarks.Value = foundRow(0).Item("remarks").ToString
                dl_notarizedFormWithID.Value = foundRow(0).Item("notarizedIDpicture").ToString
                dl_fullBodyID.Value = foundRow(0).Item("fullBodyID").ToString
                txt_priReq.Value = foundRow(0).Item("primaryReq").ToString
                txt_secReq.Value = foundRow(0).Item("secondReq").ToString
                populateMultiple.Value = If(foundRow(0).Item("secondReq") IsNot Nothing, "1", "0").ToString
                select_status.Value = foundRow(0).Item("status").ToString


                Dim bDate As DateTime = DateTime.ParseExact(txt_BDate.Value.Trim, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                Dim age As Integer = .dateNow.Year - bDate.Year

                If age >= 80 AndAlso age <= 100 Then
                    div_attachment.Visible = True
                    div_attachment.Attributes("class") = div_attachment.Attributes("class").Replace("hide", "").Trim()
                    dl_notarizedFormWithID.Attributes("class") &= " required"
                    dl_fullBodyID.Attributes("class") &= " required"
                Else
                    div_attachment.Visible = False
                    div_attachment.Attributes("class") &= " hide"
                End If

                If .checkUser = 1 Or .checkUser = 2 Then
                    If foundRow(0).Item("Status").ToString = 1 Then
                        btn_submit.Visible = True
                    ElseIf foundRow(0).Item("Status").ToString = 0 Then
                        btn_submit.Visible = True
                    ElseIf foundRow(0).Item("Status").ToString = 2 Then
                        btn_submit.Visible = True
                    End If
                End If

                ph_timeline.Controls.Add(New Literal() With {.Text = clsMaster.loadTimeline(foundRow(0).Item("refID").ToString())})
                ClientScript.RegisterStartupScript(Me.[GetType](), "populateAttachment", "populateAttachment();", True)
            Else
                ClientScript.RegisterStartupScript(Me.[GetType](), "systemMsg", "systemMsg(0, 'Selected record did not exists.');", True)
                Response.Redirect(Master.activePage)
            End If
        End With
    End Sub

    Protected Sub Save_Click()
        If IsPostBack Then
            If requestID = Nothing Then
                CRUD("C")
            Else
                CRUD("U")
            End If
        End If
    End Sub

    Protected Sub Cancel_Click()
        CRUD("X")
    End Sub
    Protected Sub Inactive_Click()
        CRUD("I")
    End Sub

    Protected Sub Reactivate_Click()
        CRUD("R")
    End Sub


    Protected Sub CRUD(_mode As String)
        If IsPostBack Then
            Try
                With clsMaster
                    Using con As New MySqlConnection(ConnString)
                        con.Open()
                        Dim sql As String = Nothing
                        Dim refID As String = If(_mode = "C", .autoID("SNR"), txt_ownerID.Value)
                        Dim tableName As String = If(dl_type.Value = "1", "maincitizenprofile_tbl", "mainnameprofile_tbl")
                        Dim newRefID As String = Nothing
                        Dim bDate As DateTime
                        DateTime.TryParseExact(txt_BDate.Value.Trim, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, bDate)

                        If _mode = "C" Then
                            sql = "INSERT INTO osca_seniorcitizen_tbl (RefID,locationID, createDate, remarks, ncscrrn, OldRefID) values " &
                                " (@RefID,@locationID, @createDate, @Remarks,@ncscrrn, @OldRefID)"
                        ElseIf _mode = "U" Then
                            If .dynamicQuery("Select ID from maincitizenprofile_tbl WHERE RefID = '" & requestID & "'").Rows.Count > 0 Then
                                sql = "Update maincitizenprofile_tbl SET locationID =  @locationID, fName = @fName, lName = @lName, mName = @mName, suffix = @suffix, bdate = @bdate, contactNo = @contactNo, address = @address, sex = @sex, Citizenship = @Citizenship, BPlace = @BPlace, Height = @Height, Weight = @Weight, lgbtq = @lgbtq, DSWDPensioner = @DSWDPensioner, ncscrrn = @ncscrrn, Remarks = @Remarks , status = @status where RefID = @ID"
                            Else
                                newRefID = .autoID(If(dl_type.Value = "1", "CP", "CPO"))
                                sql = "INSERT INTO " & tableName & " (RefID, locationID, fName, mName, lName, suffix, bdate, contactNo, address, sex, Citizenship, age, BPlace, Height, Weight, lgbtq, DSWDPensioner, ncscrrn, Remarks, createBy, createDate, OldRefNo) values " &
                                " ('" & newRefID & "', @locationID, @fName, @mName, @lName, @suffix, @bdate, @contactNo, @address, @sex, @Citizenship, @age, @BPlace, @Height, @Weight, @lgbtq, @DSWDPensioner, @ncscrrn, @Remarks, @createBy, @createDate, @OldRefNo)"
                            End If
                        End If

                        Using cmd As New MySqlCommand(sql, con)
                            cmd.Parameters.AddWithValue("@ID", requestID)
                            cmd.Parameters.AddWithValue("@RefID", refID)
                            cmd.Parameters.AddWithValue("@OldRefID", txt_ownerID.Value.Trim)
                            cmd.Parameters.AddWithValue("@NewRefID", txt_ownerID.Value.Trim)
                            cmd.Parameters.AddWithValue("@fName", txt_FName.Value.Trim)
                            cmd.Parameters.AddWithValue("@mName", txt_MName.Value.Trim)
                            cmd.Parameters.AddWithValue("@lName", txt_LName.Value.Trim)
                            cmd.Parameters.AddWithValue("@suffix", dl_suffix.Value)
                            cmd.Parameters.AddWithValue("@locationID", txt_brgycity.Value)
                            cmd.Parameters.AddWithValue("@bdate", bDate)
                            cmd.Parameters.AddWithValue("@contactNo", txt_contactNo.Value)
                            cmd.Parameters.AddWithValue("@address", txt_Address.Value.Trim)
                            cmd.Parameters.AddWithValue("@lgbtq", dl_lgbtq.Value)
                            cmd.Parameters.AddWithValue("@sex", dl_sex.Value)
                            cmd.Parameters.AddWithValue("@status", select_status.Value)
                            cmd.Parameters.AddWithValue("@civilStatus", dl_civilStatus.Value)
                            cmd.Parameters.AddWithValue("@Citizenship", txt_Citizenship.Value.Trim)
                            cmd.Parameters.AddWithValue("@BPlace", txt_BPLace.Value.Trim)
                            cmd.Parameters.AddWithValue("@Height", If(txt_Height.Value = Nothing, "0", txt_Height.Value))
                            cmd.Parameters.AddWithValue("@Weight", If(txt_Weight.Value = Nothing, "0", txt_Weight.Value))
                            cmd.Parameters.AddWithValue("@DSWDPensioner", dl_dswdPentioner.Value.Trim)
                            cmd.Parameters.AddWithValue("@ncscrrn", txt_ncscRRN.Value.Trim)
                            cmd.Parameters.AddWithValue("@Remarks", txt_remarks.Value.Trim)
                            cmd.Parameters.AddWithValue("@createBy", Master.userID)
                            cmd.Parameters.AddWithValue("@updateBy", Master.userID)
                            cmd.Parameters.AddWithValue("@brgyID", txt_brgycity.Value)
                            cmd.Parameters.AddWithValue("@createDate", .dateNow)

                            cmd.ExecuteNonQuery()
                        End Using

                        con.Close()
                        con.Dispose()

                        If Not newRefID = Nothing Then
                            Dim OldProfile As String = If(dl_type.Value = "2", "maincitizenprofile_tbl", "mainnameprofile_tbl")
                            .postOperation("UPDATE " & OldProfile & " SET status = 0 WHERE refID = '" & refID & "'")
                            .addLogs(refID, "X", "Change profile type and created new profile with Ref No.: " & newRefID)
                            _mode = "C"
                            refID = newRefID
                        End If

                        Dim remarks As String = Nothing
                        If Not String.IsNullOrEmpty(txt_reactivateRemarks.Value) Then
                            remarks = txt_reactivateRemarks.Value
                            _mode = "U"
                        End If

                        'NEW UPDATING METHOD 06/27/25
                        If Not String.IsNullOrEmpty(Master.CitizenUpdatesJson) Then
                            remarks += "|json|" & Master.CitizenUpdatesJson
                        End If

                        'BULLET Type UPDATING METHOD 06/27/25 
                        If Not String.IsNullOrEmpty(txt_updates.Value) Then
                            Dim formattedChanges As String = FormatChanges(txt_updates.Value)
                            remarks &= vbCrLf & vbCrLf & formattedChanges
                        End If

                        Session("action") = _mode
                        .addLogs(If(requestID IsNot Nothing, requestID, txt_ownerID.Value), _mode, remarks)

                        If Not newRefID = Nothing Then
                            Response.Redirect(.activePage + "?ID=" + .Encrypt(refID))
                        Else
                            Response.Redirect(Request.RawUrl)
                        End If
                    End Using
                End With
            Catch ex As Exception
                ClientScript.RegisterStartupScript(Me.[GetType](), "systemMsg", "systemMsg(0, '" & ex.Message.Replace("'", "|") & "');", True)
            End Try
        End If
    End Sub


    Private Function FormatChanges(jsonString As String) As String
        Try
            Dim changes As List(Of Dictionary(Of String, String)) = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of Dictionary(Of String, String)))(jsonString)
            Dim formattedList As New List(Of String)

            For Each change As Dictionary(Of String, String) In changes
                formattedList.Add("<li>Changed from '<em>" & change("oldValue") & "</em>' to '<em>" & change("newValue") & "</em>'</li>")
            Next

            Return "<ul>" & String.Join("", formattedList) & "</ul>"
        Catch ex As Exception
            Return "<p>Error parsing changes</p>"
        End Try
    End Function




    <WebMethod>
    Public Shared Function LoadList1(mode As String, status As String, age As String, brgy As String, DSWDPensioner As String, sex As String, keyword As String) As String
        Dim clsMaster As New cls_master
        Dim htmlStr As New StringBuilder
        Try
            With clsMaster
                Dim filterList As New List(Of String)

                If mode = 0 Then
                    filterList.Add("bDate BETWEEN DATE_SUB(CURDATE(), INTERVAL 100 YEAR) AND DATE_SUB(CURDATE(), INTERVAL 60 YEAR)")

                    If Not String.IsNullOrEmpty(age) AndAlso age.Contains("-") Then
                        Dim ageParts As String() = age.Split("-"c)
                        Dim minAge As Integer = Convert.ToInt32(ageParts(0))
                        Dim maxAge As Integer = Convert.ToInt32(ageParts(1))

                        filterList.Add("TIMESTAMPDIFF(YEAR, COALESCE(m.bDate, CURDATE()), CURDATE()) BETWEEN '" & minAge & "' AND '" & maxAge & "'")
                    End If

                    If brgy IsNot Nothing AndAlso brgy.Trim() <> "" Then
                        filterList.Add("m.locationID IN (" & brgy.Trim() & ")")
                    End If

                    If sex IsNot Nothing AndAlso sex.Trim() <> "" Then
                        filterList.Add("m.sex IN (" & sex.Trim() & ")")
                    End If

                    If Not status = Nothing And Not status = "Last" Then
                        filterList.Add("m.Status = '" & status & "'")
                    End If

                    If Not DSWDPensioner = Nothing AndAlso DSWDPensioner.Trim() <> "" Then
                        filterList.Add("m.DSWDPensioner = '" & DSWDPensioner.Trim() & "'")
                    End If
                Else
                    Dim keywordConditions As New List(Of String)
                    keywordConditions.Add("m.RefID LIKE '%" & keyword.Trim() & "%'")
                    keywordConditions.Add("TRIM(CONCAT(COALESCE(m.fName, ''), ' ', COALESCE(m.mName, ''), ' ', COALESCE(m.lName, ''), ' ', COALESCE(m.suffix, ''))) LIKE '%" & keyword.Trim() & "%'")
                    keywordConditions.Add("b.Name LIKE '%" & keyword.Trim() & "%'")

                    Dim ageFiltering As String = "(" & String.Join(" OR ", keywordConditions) & ")"
                    ageFiltering &= " AND TIMESTAMPDIFF(YEAR, COALESCE(m.bDate, CURDATE()), CURDATE()) BETWEEN 60 AND 100"

                    filterList.Add(ageFiltering)
                End If

                Dim conditions As String = String.Join(" AND ", filterList)
                Dim orderBy As String = If(mode = 0 AndAlso status = "Last", " ORDER BY m.createDate DESC LIMIT 10", "")
                Dim foundDT As DataTable = .dynamicTbl("SeniorOSCA", conditions & orderBy, "sp_dynamicDT")

                If foundDT.Rows.Count > 0 Then
                    For Each row In foundDT.AsEnumerable
                        Dim statusValue As String = row.Item("status").ToString()
                        Dim badgeClass As String = ""
                        Dim locationName As String = GetLocationNameByID(row.Item("locationID").ToString(), "1")

                        Select Case statusValue
                            Case "1"
                                badgeClass = "<span class='badge badge-success'>Active</span>"
                            Case "2"
                                badgeClass = "<span class='badge badge-danger'>Cancelled</span>"
                            Case "0"
                                badgeClass = "<span class='badge badge-secondary'>Inactive</span>"
                            Case Else
                                badgeClass = "<span class='badge badge-warning'>Unknown</span>"
                        End Select

                        htmlStr.Append("<tr>")
                        htmlStr.Append("<td style='width:7%;'><a href=""" + .activePage + "?ID=" + .Encrypt(row.Item("RefID").ToString) + """ Class=""green""><i class=""fa fa-search bigger-130""></i> &nbsp" + row.Item("RefID").ToString.ToUpper + "</a></td>")
                        htmlStr.Append("<td style='width:15%;'>" + row.Item("fullName").ToString.ToUpper + "</td>")
                        htmlStr.Append("<td style='width:3%;'>" + row.Item("sex").ToString.ToUpper + "</td>")
                        htmlStr.Append("<td style='width:10%;'>" + row.Item("BirthDate").ToString.ToUpper + "</td>")
                        htmlStr.Append("<td class='text-center' style='width:1%;'>" + row.Item("age").ToString.ToUpper + "</td>")
                        htmlStr.Append("<td style='width:7%;'>" & locationName.ToUpper() & "</td>")
                        htmlStr.Append("<td class='text-center' style='width:7%;'>" + row.Item("ncscrrn").ToString.ToUpper + "</td>")
                        htmlStr.Append("<td class='text-center' style='width:8%;'>" & If(
                            Not IsDBNull(row.Item("DSWDPensioner")) AndAlso Not String.IsNullOrEmpty(row.Item("DSWDPensioner").ToString()),
                            If(row.Item("DSWDPensioner").ToString() = "1", "Not Indicated",
                                If(row.Item("DSWDPensioner").ToString() = "0", "Not a Pensioner",
                                If(row.Item("DSWDPensioner").ToString() = "2", "Regional",
                                If(row.Item("DSWDPensioner").ToString() = "3", "Subsistence Assistance",
                                If(row.Item("DSWDPensioner").ToString() = "4", "City", ""))))),
                            "") & "</td>")
                        htmlStr.Append("<td class='text-center' style='width:3%;'>" & badgeClass & "</td>")
                        htmlStr.Append("</tr>")
                    Next
                End If
            End With

            Return htmlStr.ToString
        Catch ex As Exception
            Return "Error: " + ex.Message.Replace("'", "|")
        End Try

    End Function
    Function GetStatusLabel(status As String) As String
        Select Case status
            Case "0"
                Return "Inactive"
            Case "1"
                Return "Reactivated"
            Case "2"
                Return "Cancelled"
            Case Else
                Return "Unknown"
        End Select
    End Function

    <WebMethod>
    Public Shared Function populateDynamicDl(type As String, selections As String) As List(Of ListItem)
        Dim clsMaster2 As New cls_master
        Dim dlList As New List(Of ListItem)

        With clsMaster2
            Dim filterList As New List(Of String)
            filterList.Add("Status = 1")
            If Not selections = Nothing Then
                For Each row In .deserializeJSON(selections).AsEnumerable
                    filterList.Add("RefID <> '" & row.Item("col2").ToString & "'")
                Next
            End If

            Dim dynamicDT As New DataTable

            If type = "1" Then
                dynamicDT = .dynamicQuery("Select bpl_profile_tbl.RefID, Concat('(', bpl_profile_tbl.RefID, ') ', bpl_profile_tbl.Name) As Name From bpl_profile_tbl" &
                    " Where " & String.Join(" AND ", filterList) & " AND bpl_profile_tbl.Name <> '' Order By bpl_profile_tbl.Name")
            ElseIf type = "2" Then
                dynamicDT = .dynamicQuery("Select rpt_assessmentall_tbl.RefID, Concat('(', rpt_assessmentall_tbl.TDNo, ') ', rpt_assessmentall_tbl.CustomOwnerName) As Name From rpt_assessmentall_tbl" &
                    " Where " & String.Join(" AND ", filterList) & " Order By Name")
            End If

            For Each row In dynamicDT.AsEnumerable
                dlList.Add(New ListItem() With {
                    .Value = row.Item("RefID").ToString(),
                    .Text = row.Item("Name").ToString.Trim.Replace("'", "`").Replace("/", "//").ToUpper
                })
            Next
        End With

        Return dlList
    End Function

    <WebMethod>
    Public Shared Function getLocations(type As String) As List(Of ListItem)
        Dim clsMaster2 As New cls_master
        Dim dlList As New List(Of ListItem)
        Dim selectedDT As New DataTable

        With clsMaster2
            Dim locationDR As DataTable
            Dim sqlQuery As String

            If type = "1" Then
                sqlQuery = "SELECT " &
                       "b.Name AS BarangayName, " &
                       "l.Name AS LocationName, " &
                       "l.ID " &
                       "FROM mainbrgy_tbl b " &
                       "INNER JOIN rpt_locations_tbl l ON b.RefID = l.BrgyID " &
                       "WHERE b.Status = 1 AND l.Status = 1 " &
                       "ORDER BY b.Name, l.Name"
            Else
                sqlQuery = "SELECT Name AS LocationName, ID " &
                       "FROM rpt_locations_tbl " &
                       "WHERE Status = 1 " &
                       "ORDER BY Name"
            End If

            locationDR = .dynamicQuery(sqlQuery)

            For Each row In locationDR.Rows
                Dim concatenatedName As String

                If type = "1" Then
                    concatenatedName = row.Item("BarangayName").ToString() & " - " & row.Item("LocationName").ToString()
                Else
                    concatenatedName = row.Item("LocationName").ToString()
                End If

                dlList.Add(New ListItem() With {
                .Value = row.Item("ID").ToString(),
                .Text = concatenatedName
            })
            Next
        End With

        Return dlList
    End Function

    Public Shared Function GetLocationNameByID(locationID As String, type As String) As String
        Dim locationList As List(Of ListItem) = getLocations(type)

        Dim locationName As String = locationList.
                                 Where(Function(item) item.Value = locationID).
                                 Select(Function(item) item.Text).
                                 FirstOrDefault()
        Return If(String.IsNullOrEmpty(locationName), "Unknown Location", locationName)
    End Function

    Public Shared Function GetBrgyNameByID(brgyID As String) As String
        Dim connString As String = ConfigurationManager.ConnectionStrings("conn1").ConnectionString
        Dim brgyName As String = String.Empty

        Dim sql As String = "SELECT Name FROM mainbrgy_tbl WHERE Status = 1 AND ID = @brgyID"

        Using conn As New MySqlConnection(connString)
            conn.Open()

            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@brgyID", brgyID)

                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    brgyName = result.ToString()
                Else
                    brgyName = "Unknown Barangay " & brgyID
                End If
            End Using
        End Using

        Return brgyName
    End Function

    Public Shared Function GetBrgyNameFromJoin(locationID As String) As String
        Dim connString As String = ConfigurationManager.ConnectionStrings("conn1").ConnectionString
        Dim brgyName As String = String.Empty

        Dim sql As String = "SELECT b.Name AS BarangayName, l.Name AS LocationName " &
                        "FROM mainbrgy_tbl b " &
                        "INNER JOIN rpt_locations_tbl l ON b.RefID = l.BrgyID " &
                        "WHERE b.Status = 1 AND l.Status = 1 AND l.ID = @locationID"

        Using conn As New MySqlConnection(connString)
            conn.Open()

            Using cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@locationID", locationID)

                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        brgyName = reader("BarangayName").ToString() & " - " & reader("LocationName").ToString()
                    Else
                        brgyName = "Unknown Location " & locationID
                    End If
                End Using
            End Using
        End Using

        Return brgyName
    End Function

    <WebMethod>
    Public Shared Function ValidateProfile(fName As String, lName As String, bDate As DateTime) As Dictionary(Of String, String)
        Dim clsMaster2 As New cls_master
        Dim dict As New Dictionary(Of String, String)
        Try
            With clsMaster2
                Dim formattedDate As String = bDate.ToString("yyyy-MM-dd")

                If .dynamicQuery("Select der1.FName, der1.LName, der1.BDate From (Select mainnameprofile_tbl.FName, mainnameprofile_tbl.LName, mainnameprofile_tbl.BDate From mainnameprofile_tbl Where mainnameprofile_tbl.Status = 1 Union Select  maincitizenprofile_tbl.fName, maincitizenprofile_tbl.lName, maincitizenprofile_tbl.bdate From maincitizenprofile_tbl Where maincitizenprofile_tbl.status = 1) As der1 Where der1.FName = '" & fName.Trim & "' And der1.LName = '" & lName.Trim & "' And der1.BDate = '" & formattedDate & "'").Rows.Count > 0 Then
                    dict.Add("Output", 1)
                Else
                    dict.Add("Output", 0)
                End If

                Return dict
            End With
        Catch ex As Exception
            dict.Add("Error", ex.Message.Replace("'", "|").Replace("""", "|"))
            Return dict
        End Try

    End Function
    <WebMethod>
    Public Shared Function LoadList(mode As String, status As String, age As String, brgy As String, DSWDPensioner As String, sex As String, keyword As String) As String
        Dim clsMaster As New cls_master
        Dim htmlStr As New StringBuilder
        Try
            With clsMaster
                Dim filterList As New List(Of String)

                If mode = "0" Then
                    If Not String.IsNullOrEmpty(age) AndAlso age.Contains("-") Then
                        Dim ageParts As String() = age.Split("-"c)
                        Dim minAge As Integer = Convert.ToInt32(ageParts(0))
                        Dim maxAge As Integer = Convert.ToInt32(ageParts(1))
                        filterList.Add("age BETWEEN " & minAge & " AND " & maxAge)
                    End If

                    If brgy IsNot Nothing AndAlso brgy.Trim() <> "" Then
                        filterList.Add("locationID IN (" & brgy.Trim() & ")")
                    End If

                    If sex IsNot Nothing AndAlso sex.Trim() <> "" Then
                        filterList.Add("sex IN (" & sex.Trim() & ")")
                    End If

                    If Not String.IsNullOrEmpty(status) AndAlso status <> "Last" Then
                        filterList.Add("Status = '" & status & "'")
                    End If

                    If Not DSWDPensioner = Nothing AndAlso DSWDPensioner.Trim() <> "" Then
                        filterList.Add("DSWDPensioner = '" & DSWDPensioner.Trim() & "'")
                    End If
                Else
                    Dim keywordConditions As New List(Of String)
                    keywordConditions.Add("OldRefID LIKE '%" & keyword.Trim() & "%'")
                    keywordConditions.Add("RefID LIKE '%" & keyword.Trim() & "%'")
                    keywordConditions.Add("TRIM(CONCAT(COALESCE(fName, ''), ' ', COALESCE(mName, ''), ' ', COALESCE(lName, ''), ' ', COALESCE(suffix, ''))) LIKE '%" & keyword.Trim() & "%'")
                    keywordConditions.Add("fullName LIKE '%" & keyword.Trim() & "%'")

                    Dim ageFiltering As String = "(" & String.Join(" OR ", keywordConditions) & ")"
                    'ageFiltering &= " AND TIMESTAMPDIFF(YEAR, COALESCE(bDate, CURDATE()), CURDATE()) BETWEEN 60 AND 100"

                    filterList.Add(ageFiltering)
                End If

                Dim orderBy As String = ""
                If mode = "0" AndAlso status = "Last" Then
                    orderBy = " ORDER BY createdDate DESC LIMIT 10"
                End If

                Dim foundDT As DataTable = .dynamicTbl("SeniorOSCA", String.Join(If(mode = 0, " And ", " Or "), filterList) & orderBy, "sp_dynamicDT")

                If foundDT.Rows.Count > 0 Then
                    For Each row In foundDT.AsEnumerable
                        Dim statusValue As String = row.Item("status").ToString()
                        Dim badgeClass As String = ""
                        Dim brgyID As Object = row.Item("brgyID")
                        Dim locationID As Object = row.Item("locationID")

                        Dim locationName As String = GetLocationNameByID(locationID.ToString(), "1")
                        If String.IsNullOrEmpty(locationName) OrElse locationName = "Unknown Location" Then
                            locationName = GetBrgyNameByID(brgyID.ToString())
                        End If

                        Select Case statusValue
                            Case "1"
                                badgeClass = "<span class='badge badge-success'>Active</span>"
                            Case "2"
                                badgeClass = "<span class='badge badge-danger'>Cancelled</span>"
                            Case "0"
                                badgeClass = "<span class='badge badge-secondary'>Inactive</span>"
                            Case Else
                                badgeClass = "<span class='badge badge-warning'>Unknown</span>"
                        End Select

                        htmlStr.Append("<tr>")
                        htmlStr.Append("<td style='width:7%;'><a href=""" + .activePage + "?ID=" + .Encrypt(row.Item("OldRefID").ToString) + """ Class=""green""><i class=""fa fa-search bigger-130""></i> &nbsp" + row.Item("RefID").ToString.ToUpper + "</a></td>")
                        htmlStr.Append("<td style='width:15%;'>" + row.Item("fullName").ToString.ToUpper + "</td>")
                        htmlStr.Append("<td style='width:3%;'>" & If(row.Item("sex") = 1, "FEMALE", "MALE") + "</td>")
                        htmlStr.Append("<td style='width:10%;'>" + row.Item("bDate").ToString.ToUpper + "</td>")
                        htmlStr.Append("<td class='text-center' style='width:1%;'>" + row.Item("age").ToString.ToUpper + "</td>")
                        htmlStr.Append("<td style='width:7%;'>" & locationName & "</td>")
                        htmlStr.Append("<td class='text-center' style='width:7%;'>" + row.Item("ncscrrn").ToString.ToUpper + "</td>")
                        htmlStr.Append("<td class='text-center' style='width:8%;'>" & If(
                            Not IsDBNull(row.Item("DSWDPensioner")) AndAlso Not String.IsNullOrEmpty(row.Item("DSWDPensioner").ToString()),
                            If(row.Item("DSWDPensioner").ToString() = "1", "Not Indicated",
                                If(row.Item("DSWDPensioner").ToString() = "0", "Not a Pensioner",
                                If(row.Item("DSWDPensioner").ToString() = "2", "Regional",
                                If(row.Item("DSWDPensioner").ToString() = "3", "Subsistence Assistance",
                                If(row.Item("DSWDPensioner").ToString() = "4", "City", ""))))),
                            "") & "</td>")
                        htmlStr.Append("<td class='text-center' style='width:3%;'>" & badgeClass & "</td>")
                        htmlStr.Append("</tr>")
                    Next
                End If
            End With

            Return htmlStr.ToString
        Catch ex As Exception
            Return "Error: " + ex.Message.Replace("'", "|")
        End Try

    End Function
End Class