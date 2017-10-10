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
    var balance  = Number(data.Balance.replace(/[^0-9\.-]+/g,"")); 
    var tax = balance * .1;
    var totalRcv = balance - tax;

    tmpl = tmpl.replace('[fullname]', data.FirstName + ' ' + data.MI + ' ' + data.LastName);
    tmpl = tmpl.replace('[acntName]', data.FirstName + ' ' + data.MI + ' ' + data.LastName);
    tmpl = tmpl.replace('[gender]',data.Gender =='M'?'Male':'Femail');
    tmpl = tmpl.replace('[address]',data.Street + ' ' + data.CityProvince);
    tmpl = tmpl.replace('[contact]',data.ContactNo);
    tmpl = tmpl.replace('[AccountID]', data.AccountID);
    tmpl = tmpl.replace('[email]', data.Username);
    
    
    tmpl = tmpl.replace('[pairingbonus]', data.PairingBunos);
    tmpl = tmpl.replace('[referrals]', data.RefferralBunos);
    tmpl = tmpl.replace('[withdrawals]', data.TotalWithdrawals);
    tmpl = tmpl.replace('[balance]', data.Balance);

    tmpl = tmpl.replace('[wdbalance]', data.Balance);
    tmpl = tmpl.replace('[Tax]', tax.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    tmpl = tmpl.replace('[TotalRcv]', totalRcv.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    
    $("#profile").html(tmpl);
}

function WithdrawalStep1() {
    $('#Withdrawalslip').show();
    $('#accountdetails').hide();
}
function HideWithdrawal() {
    $('#Withdrawalslip').hide();
    $('#accountdetails').show();
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