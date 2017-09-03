$(window).ready(function () {
    requestGenealogy(jsUID);

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

    $.each(data, function(i,rec) {
        //-- only append the parent on first record
         console.log(rec)
        if(i==0){
            console.log(i)
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
        
        //-- limit up to 6 and assumes records passed is in sequence
        if(i==6){
            return false;
        }
    });

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
  return  '<li><a href="#">' +
        '<div class="ctrlContainer">' +
            '<img class="img-responsive ctrlHeight" src="./images/plusgreen.png"></img>' +
        '</div>' +
        '<div class="ctrlContainer">' +
            '<img class="img-responsive ctrlHeight" src="./images/UserPlus.png"></img>' +
        '</div>' +
    '</a></li>';
}

function tmpRed(data){
   return '<li id="'+data.GenealogyID+'">'+
           '<a href="#">' +
                '<img class="img-responsive" src="./images/if_User_Yuppie_3_1218716.png"></img>' +
                '<label>'+data.AccountName+'</label>' +
            '</a></li>';
}

function tmpBlue(data){
   return '<li id="'+data.GenealogyID+'">'+
            '<a href="#">'+
                '<img class="img-responsive" src="./images/if_User_Generic_1_1218733.png"></img>' +
                '<label>'+data.AccountName+'</label>' +
            '</a></li>';
}