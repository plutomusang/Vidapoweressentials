var parentHistoryContainer = [];

$(window).ready(function () {
    requestGenealogy(jsUID);


    $("#contact_form").submit(function(e){
        e.preventDefault();
        var storedProcname = "Insert_Membership";

        

        var postdata=$('#contact_form').serialize() +'&StoredProc='+storedProcname+'&key='+jsToken;
        var obj = {}; 
        postdata.replace(/([^=&]+)=([^&]*)/g, function(m, key, value) {
            obj[decodeURIComponent(key)] = decodeURIComponent(value);
        }); 

        $.post(baseUrl() + "api/powercall",obj, 
            function (data) {
            
            }
        , 'json');

    });


    $('#cancel').click(function(e){
        $('#Assign-downline').modal('hide');
        $("#contact_form")[0].reset()
    });
});


function requestGenealogy(genealogyID){
    var storedProcname = "Get_Genealogy";
    $.get(baseUrl() + "api/Genealogy", { 'StoredProc': storedProcname,'GenealogyID':genealogyID, 'key': jsToken }, function (data) {
        buildTree(data);
    }, 'json');
}


function buildTree(rawData){

    var data = rawData.Get_Genealogy;
    var elementTag = '';
    var parentId ='';
    var levelCount =0;

    $.each(data, function(i,rec) {
        //-- only append the parent on first record
        // console.log(rec)
        if(i==0){
            //-- lets determine color thru package
            if(rec.Package =='A'){
                elementTag = tmpBlue(rec);
            }else{
                elementTag = tmpRed(rec);
            }
            $('#treeChart').append(elementTag);

        }
        //-- lets get child
        getChild(rec.GenealogyID,data);

        levelCount = levelCount+1;
        
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

    $.each(data, function(i,rec) {
        //-- lets get the child according to id
        if(rec.Mothernode == parentId){
            //-- lets determine color thru package
            if(rec.Package =='A'){
                tmpElement = tmpBlue(rec);
            }else{
                tmpElement = tmpRed(rec);
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
        leftElement =  tmpAdd();
        rightElement =  tmpEmpty();
    }else{
        leftElement =  leftElement==''?tmpAdd():leftElement;
        rightElement =  rightElement==''?tmpAdd():rightElement;
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

function tmpAdd(data){
  return  '<li><a href="#" style="cursor: default" >' +
        '<div class="ctrlContainer">' +
            '<img style="cursor: pointer" onClick="addDownline(0)" class="img-responsive ctrlHeight" src="./images/plusgreen.png"></img>' +
        '</div>' +
        '<div class="ctrlContainer">' +
            '<img style="cursor: pointer" onClick="addDownline(1)" class="img-responsive ctrlHeight" src="./images/UserPlus.png"></img>' +
        '</div>' +
    '</a></li>';
}

function tmpRed(data){
   return '<li id="'+data.GenealogyID+'">'+
           '<a href="#" onClick = "regenerate('+data.GenealogyID+');">' +
                '<img class="img-responsive" src="./images/if_User_Yuppie_3_1218716.png"></img>' +
                '<label>'+data.AccountName+'</label>' +
            '</a></li>';
}

function tmpBlue(data){
   return '<li id="'+data.GenealogyID+'">'+
            '<a href="#" onClick = "regenerate('+data.GenealogyID+');">'+
                '<img class="img-responsive" src="./images/if_User_Generic_1_1218733.png"></img>' +
                '<label>'+data.AccountName+'</label>' +
            '</a></li>';
}

function regenerate(genId){
     $('#treeChart li').remove();
     requestGenealogy(genId);

}

function addDownline(isNew){

    if(isNew ==1){
        $('#new_section').show();
    }else{
        $('#new_section').hide();
    }

    $('#Assign-downline').modal('show');
}


function parentHistory(liElement){
    if(parentHistoryContainer.length ==0){
        parentHistoryContainer.push()
    }
}