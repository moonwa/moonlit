(function() {
  $(function() {
    return window.submit_with_action = function(formAction, name, value) {
      var button, form;
      if (!formAction) {
        button = $("<button style='display:none' />");
      } else {
        button = $("<button style='display:none' name='form_action' value='" + formAction + "' />");
      }
      form = $("body form");
      form.append(button);
      if (name != null) {
        if (name) {
          if ($("[name='" + name + "']", form).length === 0) {
            form.append($("<input type='hidden' name='" + name + "' value='" + value + "' />"));
          } else {
            $("[name='" + name + "']", form).val(value);
          }
        }
      }
      return button.click();
    };
  });

}).call(this);
