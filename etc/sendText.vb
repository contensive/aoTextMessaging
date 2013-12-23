function m
	dim form,cs,list
	set form = cp.blockNew()
	set cs = cp.csNew()
	'
	call form.openLayout( "Send Text Message Form" )
	'
	list = ""
	if cs.open( "groups" ) then
		do
			list = vbcrlf & vbtab & "<li><label><input type=""checkbox"" name=""group" & ptr & """ value=""" & id & """>" & cs.gettext( "caption" ) & "</label></li>"
			call cs.gonext()
		loop while cs.ok
	end if
	call cs.close
	call form.setInner("#stGroupList", list )
	'
	m = form.getHtml()
end function