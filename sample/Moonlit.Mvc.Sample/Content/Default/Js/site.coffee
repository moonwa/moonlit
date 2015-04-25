# CoffeeScript
$ () -> 
    window.submit_with_action = (formAction, name, value) ->
        if !formAction
            button = $ "<button style='display:none' />" 
        else
            button = $ "<button style='display:none' name='form_action' value='#{formAction}' />"
            
        form = $ "body form"
        form.append(button)  

        if name?
            if name
                if $("[name='#{name}']", form).length is 0
                    form.append $ "<input type='hidden' name='#{name}' value='#{value}' />"
                else
                    $("[name='#{name}']", form).val value
            
        button.click();  
        
#    $("body").on "click", ".sub-menu-header", (e) -> 
#        parent = $(this).parent()
#        $submenu = $ ".sub-menu", parent
#        $submenu.show() 
        
#    $("textarea.input-html").each (x)->
#        editer = CKEDITOR.replace(this,  {
#            filebrowserUploadUrl: '/uploader/upload'
#        });
