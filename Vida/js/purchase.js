$(window).ready(function () {

    $('.datepicker').datepicker( "option", "dateFormat", "yyyy-mm-dd" );

    $('#frmSearch').submit(function (event) {
        event.preventDefault();
        searchName();
    });

    $('#btnSubmitReg').click(function(e){
        submitRegistration();

    });

     $('#btnAdd').click(function (event) {
        event.preventDefault();
        $('#first_name').val($('#searchName').val());
        $('#dvNew').show();
         $('#searchSection').hide();
          $('#dvResult').hide();
    });

     var navListItems = $('div.setup-panel div a'),
              allWells = $('.setup-content'),
              allNextBtn = $('.nextBtn');

      allWells.hide();

      navListItems.click(function (e) {
          e.preventDefault();
          var $target = $($(this).attr('href')),
                  $item = $(this);

          if (!$item.hasClass('disabled')) {
              navListItems.removeClass('btn-primary').addClass('btn-default');
              $item.addClass('btn-primary');
              allWells.hide();
              $target.show();
              $target.find('input:eq(0)').focus();
          }
      });

      allNextBtn.click(function(){
          var curStep = $(this).closest(".setup-content"),
              curStepBtn = curStep.attr("id"),
              nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
              curInputs = curStep.find("input[type='text'],input[type='url']"),
              isValid = true;

          $(".form-group").removeClass("has-error");
          for(var i=0; i<curInputs.length; i++){
              if (!curInputs[i].validity.valid){
                  isValid = false;
                  $(curInputs[i]).closest(".form-group").addClass("has-error");
              }
          }

          if (isValid)
              nextStepWizard.removeAttr('disabled').trigger('click');
      });

      $('div.setup-panel div a.btn-primary').trigger('click');

});


function searchName() {
    var storedProcname = "searchMember";
    var postdata='Name='+$('#searchName').val()+'&StoredProc='+storedProcname+'&key='+jsToken;

    var obj = {}; 
    postdata.replace(/([^=&]+)=([^&]*)/g, function(m, key, value) {
        obj[decodeURIComponent(key)] = decodeURIComponent(value);
    }); 

    $.post(baseUrl() + "api/powercall",obj, 
        function (rawdata) {
            var data = rawdata.searchMember;
            resetRecord();
            if(data.length >0){
                populateTable(data)
            }else{
                $('#tblResult > tbody:last-child').append('<tr><td>No Result</td><td></td><td></td><td></td></tr>');
            }
            $('#dvResult').show();
        }
    , 'json');
}



function submitRegistration(){
     var storedProcname = "Insert_POS_Membership";

        var postdata=$('#contact_form').serialize() +'&Referer='+jsUID+'&StoredProc='+storedProcname+'&key='+jsToken;
        var obj = {}; 
        postdata.replace(/([^=&]+)=([^&]*)/g, function(m, key, value) {
            obj[decodeURIComponent(key)] = decodeURIComponent(value);
        }); 

        $.post(baseUrl() + "api/powercall",obj, 
            function (data) {

                console.log(data.Insert_POS_Membership[0].Status)
                if(data.Insert_POS_Membership[0].Status =='Success'){
                    $('#contact_form')[0].reset();
                    resetRecord();
                    $('#dvNew').hide();
                     $('#searchSection').show();
                      $('#dvResult').hide();
                      $('#firstStep').trigger('click');
                       $('#succesMsg').show().delay(3000).fadeOut();;

                }else{
                    $('#errorMsg').show().delay(3000).fadeOut();
                }
            }
        , 'json');
}


function resetRecord(){
    $('#tblResult > tbody').empty();
}


function populateTable(dataArr){
    console.log(dataArr)
    //-- specify the delimeter
    dataArr.forEach(function(item,index){
        //--lets process items with string only
        if(item.length !=0){
            var tr = '<tr><td>'+item.Name+'</td><td></td>'+
                                '<td><select name="packageResult" class="form-control">'+
                                    '<option>Elite</option>'+
                                    '<option>Economy</option>'+
                                '</select></td><td>'+
                                
             '<a href="reOrder()" class="btn btn-primary pull-right"><i class="fa fa-check"></i></a></td></tr>';
            $('#tblResult > tbody:last-child').append(tr);
        }
    });
}


function reOrder(id){

}