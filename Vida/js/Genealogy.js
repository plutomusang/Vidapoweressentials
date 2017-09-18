var GeneralData;
var currentParent;
var focusId ;
var newEntry;
var memberID;
$(window).ready(function () {
     requestGenealogy(jsUID);
     focusId = jsUID;

    $("#btnSubmit").click(function(e){
        e.preventDefault();
       
        submitValidate();
    });


    $('#cancel').click(function(e){
        $('#Assign-downline').modal('hide');
        $("#contact_form")[0].reset()
        $("#btnNext").addClass("hidden");
        $('#packageType').html('');
    });

    $('#btnVerify').click(function (e) {
        e.preventDefault();
        var storedProcname = "Validate_Membership";
        var vCode = $('#vCode').val(); //'001c9e0sbzszfsk';//

        var postdata = 'StoredProc=' + storedProcname + '&key=' + jsToken + '&first_name= &last_name= &Birth_date= &ValidationCode=' + vCode;
        var obj = {};
        postdata.replace(/([^=&]+)=([^&]*)/g, function (m, key, value) {
            obj[decodeURIComponent(key)] = decodeURIComponent(value);
        });

        $.post(baseUrl() + "api/powercall", obj,
            function (data) {

                var rec = data["Validate_Membership"];
                rec = rec[0];
                //-- Lets select the right package label 
                $('#packageType').html(rec.ValidationCodeErr);
                if (rec.PackageType == 'A') { $('#packageType').html('Validated Elite Package'); }
                if (rec.PackageType == 'B') { $('#packageType').html('Validated Economy Package'); }
                $('#mPackageType').val( rec.PackageType );

                    if (rec.ValidationCodeErr == ''){
                        $('#footerM').show();
                    }
            }
        , 'json');

    });

});


function submitValidate(){
     var storedProcname = "Insert_Membership";

        var postdata=$('#contact_form').serialize() +'&StoredProc='+storedProcname+'&key='+jsToken;
        var obj = {}; 
        postdata.replace(/([^=&]+)=([^&]*)/g, function(m, key, value) {
            obj[decodeURIComponent(key)] = decodeURIComponent(value);
        }); 

        $.post(baseUrl() + "api/powercall",obj, 
            function (data) {
                if(data.Insert_Membership.length>0){
                     $('#footerM').hide();
                     $('#Assign-downline').modal('hide');
                      $('#vCode').val('');
                      regenerate(focusId);
                }
            }
        , 'json');
}


function requestGenealogy(genealogyID){
    var storedProcname = "Get_Genealogy";
    $.get(baseUrl() + "api/Genealogy", { 'StoredProc': storedProcname,'GenealogyID':genealogyID, 'key': jsToken }, function (data) {
        buildTree(data);
        requestNewList(jsUID);
    }, 'json');
}


function requestNewList(genealogyID){
    var storedProcname = "Get_NewRegisteredMembers";
    $.get(baseUrl() + "api/Genealogy", { 'StoredProc': storedProcname,'MemberID':genealogyID, 'key': jsToken }, function (data) {
        if(data.Get_NewRegisteredMembers.length >0){
            renderTable(data.Get_NewRegisteredMembers);
        }
    }, 'json');
}

function renderTable(dataArr){

    $('#tblResult > tbody').empty();
    dataArr.forEach(function(item,index){
        //--lets process items with string only
        if(item.length !=0){
            var tr = '<tr><td>'+item.FirstName+' '+item.LastName+'</td><td>'+item.VerificationCode+'</td>'+
                                '<td><a href="javascript:userVcode(\''+item.VerificationCode+'\');" class="btn btn-primary pull-right"><i class="fa fa-check"></i></a></td></tr>';
            $('#tblResult > tbody:last-child').append(tr);
        }
    });
}

function userVcode(vCode){
    $('#vCode').val(vCode);
    return;
}




function buildTree(rawData){

    var data = rawData.Get_Genealogy;
    var elementTag = '';
    var parentId ='';
    GeneralData = data;

    $.each(data, function(i,rec) {
        //-- only append the parent on first record
        // console.log(rec)
        if(i==0){
            //-- lets determine color thru package
            if(rec.Package =='A'){
                elementTag = tmpBlue(rec,'');
            }else{
                elementTag = tmpRed(rec,'');
            }
            $('#treeChart').append(elementTag);

        }
        //-- lets get child
        if (rec.GenealogyID == jsUID) memberID = rec.memberID;
        getChild(rec.GenealogyID,data);
        
        //-- limit up to 6 and assumes records passed is in sequence
        if(i==6){
            return false;
        }
    });

    //-- if tree is not complete add empty
    //console.log(levelCount);

}

function getChild(parentId, data){
  var leftElement ='';
  var rightElement = '';
  var tmpElement ='';
  var parentAccountName;

    $.each(data, function(i,rec) {
        //-- lets get the child according to id
        if (rec.GenealogyID == parentId) parentAccountName = rec.AccountName;
        if(rec.Mothernode == parentId){
            //-- lets determine color thru package
            if(rec.Package =='A'){
                tmpElement = tmpBlue(rec,'');
            }else{
                tmpElement = tmpRed(rec,'');
            }

            //-- lets check if left or right
            if(rec.Position =='L'){
                leftElement = tmpElement;
            }else{
                rightElement = tmpElement;
            }
     //       console.log(rec);
        }
    });

    //-- lets check if left is empty
    if(leftElement =='' && rightElement ==''){
        leftElement = tmpAdd(parentAccountName, 'L');
        rightElement = tmpAdd(parentAccountName, 'R');
    }else{
        leftElement = leftElement == '' ? tmpAdd(parentAccountName, 'L'):leftElement;
        rightElement = rightElement == '' ? tmpAdd(parentAccountName, 'R'):rightElement;
    }
    $('#'+parentId).append('<ul>'+leftElement +rightElement +'</ul>');

}

function tmpEmpty(){
    return ' <li>' +
                '<a href="#">' +
                    '<div class="ctrlContainer">' +
                    '</div>' +
                    '<div class="ctrlContainer">' +
                    '</div>' +
                '</a>' +
            '</li>';
}

function tmpAdd(acntName, pos) {
    acntName = "'" + $.trim(acntName) + "'";
    pos = "'" + $.trim(pos) + "'";
  return  '<li><a href="#" style="cursor: default" >' +
        '<div class="ctrlContainer">' +
      '<img style="cursor: pointer" onClick="addDownline(0,' + acntName + ',' + pos +')"  class="img-responsive ctrlHeight" src="/images/plusgreen.png"></img>' +
        '</div>' +
    '</a></li>';
}

function tmpRed(data,prefix){
   return '<li id="'+prefix+data.GenealogyID+'">'+
           '<a href="#" onClick = "'+prefix+'regenerate('+data.GenealogyID+');">' +
                '<img class="img-responsive" src="/images/if_User_Yuppie_3_1218716.png"></img>' +
                '<label>'+data.AccountName+'</label>' +
            '</a></li>';
}

function tmpBlue(data,prefix){
   return '<li id="'+prefix+data.GenealogyID+'">'+
            '<a href="#" onClick = "'+prefix+'regenerate('+data.GenealogyID+');">'+
                '<img class="img-responsive" src="/images/if_User_Generic_1_1218733.png"></img>' +
                '<label>'+data.AccountName+'</label>' +
            '</a></li>';
}

function regenerate(genId){
    parentHistory();
     $('#treeChart li').remove();
     requestGenealogy(genId);
     focusId = genId;

}

function historyregenerate(genId){
    
    var bookmark = false;
    $('#historyTree li').each(function( index ) {
        if($(this).attr('id') =='history'+genId){
            bookmark = true;
        }
        if(bookmark===true){
            $(this).remove();
        }
    });


     $('#treeChart li').remove();
     requestGenealogy(genId);


}

function addDownline(isNew, accntname, pos) {
    newEntry = isNew;

    $('#assignTitle').html("Assign Account");

    $('#packageType').html('');
    $('#new_section').hide();
    $('#Assign-downline').modal('show');

    $('#mDirect_Upline_ID').val(accntname );
    $('#mGroup').val( pos );
    $('#mReferer').val( jsUID );
    
}


function parentHistory(){
    var tmpElement ='';
    var parentId=$('#treeChart li' ).first().attr('id');
    //--- already parent
    if(parentId == currentParent){
     //   return;
    }
    currentParent = parentId;

     $.each(GeneralData, function(i,rec) {
        //-- lets get the child according to id
        // console.log(rec.GenealogyID +' = '+ parentId)
        if(rec.GenealogyID == parentId){
            //-- lets determine color thru package
            if(rec.Package =='A'){
                tmpElement = tmpBlue(rec,'history');
            }else{
                tmpElement = tmpRed(rec,'history');
            }
        }
    });

     //-- check if already exist 
     if(!$('#history'+parentId).length){
        $('#historyTree').append(tmpElement);
    }
}