
option explicit

function m
	dim form,cs,list,ptr,cnt,message,groupIdList,messageTemplate,groupId,sql,fromAddress,progress,js,emailExtension,toAddress
	set form = cp.blockNew()
	set cs = cp.csNew()
	m = ""
	progress=""
	'
	cnt = cp.doc.getinteger( "groupCnt" )
	if cnt>0 then
		'
		' form submitted
		'
		groupIdList = ","
		for ptr=0 to cnt-1
			groupId = cp.doc.getInteger( "group" & ptr )
			if groupId<>0 then
				groupIdList = groupIdList & groupId & ","
			end if
		next
		messageTemplate = cp.doc.getText( "message" )
		fromAddress = cp.doc.getText( "fromAddress" )
		if ( groupIdList<>"," ) and ( messageTemplate <> "" ) then
			
			groupIdList = mid( groupIdList, 2, len( groupIdList )-2 )
			sql = "select u.*,p.emailExtension from ccmembers u left join mobileProviders p on p.id=u.mobileProviderId where u.id in (select distinct u.id from ccmembers u left join ccmemberrules r on r.memberid=u.id where (u.cellphone is not null) and (r.groupid in (" & groupIdList & ")))"
			if cs.openSql( sql ) then
				do
					emailExtension = cs.getText( "emailExtension" )
					toAddress =  cs.gettext( "cellphone" ) & emailExtension
					message = encodeMessage( cs, messageTemplate )
					progress = progress & "<div>sending message to " & cs.getText( "name" ) & " at [" & toaddress & "], [" & message & "]</div>"
					call cp.email.send( toAddress, fromAddress, "", message )
					call cs.gonext()
				loop while cs.ok()
			end if
			call cs.close()
		end if
	else
		'
		' no submit
		'
	end if
	'
	call form.openLayout( "Send Text Message Form" )
	'
	list = ""
	ptr = 0
	if cs.open( "groups" ) then
		do
			list = list & vbcrlf & vbtab & "<li><label><input type=""checkbox"" name=""group" & ptr & """ value=""" & cs.gettext( "id" ) & """>" & cs.gettext( "caption" ) & "</label></li>"
			call cs.gonext()
			ptr = ptr + 1
		loop while cs.ok
		list = list & cp.html.hidden( "groupCnt", ptr )
	end if
	call cs.close
	call form.setInner("#stGroupList", list )
	'
	if progress<>"" then
		js = js & "jQuery(document).ready(function(){jQuery('#stMessageSent').html('" & progress & "');jQuery('#stMessageSent').show();}) "
	end if
	'
	m = m & form.getHtml()
	m = cp.html.form( m, "", "", "stForm" )
	call cp.doc.addHeadJavascript( cstr( js ))
end function
'
function encodeMessage( cs, messageTemplate )
	encodeMessage = messageTemplate
end function