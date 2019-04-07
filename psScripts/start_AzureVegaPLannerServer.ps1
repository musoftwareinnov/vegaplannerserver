$ProgressPreference= "SilentlyContinue"

$password = "admudpud1"
$secpasswd = ConvertTo-SecureString $password -AsPlainText -Force
$mycreds = New-Object System.Management.Automation.PSCredential ("0b5cf302-3f75-45d2-a514-616f770c9156", $secpasswd)
Add-AzureRmAccount -ServicePrincipal -Tenant b22a1039-f55e-46d8-973f-60d92cad894d -Credential $mycreds
Select-AzureRmSubscription -SubscriptionId e69f7c40-76f1-464d-babc-0c47f2963668

Start-AzureRmWebApp -Name 'vegaPlannerServer' -ResourceGroupName 'vegaPlannerProdGroup'

