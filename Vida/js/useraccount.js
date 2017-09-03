$(window).ready(function () {

    var storedProcname = "membersprofile";
   
    $.post(baseUrl() + "api/powercall", { 'StoredProc': storedProcname, 'username': jsEmail, 'key': jsToken }, function (data) {
        getProfile(data);
        removeEmpty();
    }, 'json');
});



function getProfile(rawData){
    var tmpl = $("#profile").html();
    var data = rawData.membersprofile[0];

    tmpl = tmpl.replace('[fullname]',data.FirstName + ' '+ data.MI + ' ' + data.LastName);
    tmpl = tmpl.replace('[gender]',data.Gender =='M'?'Male':'Femail');
    tmpl = tmpl.replace('[address]',data.Street + ' ' + data.CityProvince);
    tmpl = tmpl.replace('[contact]',data.ContactNo);


     $("#profile").html(tmpl);
}


function removeEmpty(){
    const regex = /\[([a-zA-Z0-9]+)\]/gm;
    const str = $("#profile").html();
    let tmpl=$("#profile").html();

    let m;
    while ((m = regex.exec(str)) !== null) {
        // This is necessary to avoid infinite loops with zero-width matches
        if (m.index === regex.lastIndex) {
            regex.lastIndex++;
        }
        
        // The result can be accessed through the `m`-variable.
        m.forEach((match, groupIndex) => {
            if(groupIndex ==0){
                 tmpl = tmpl.replace(match,'');
            }
        });
    }

    $("#profile").html(tmpl);

}