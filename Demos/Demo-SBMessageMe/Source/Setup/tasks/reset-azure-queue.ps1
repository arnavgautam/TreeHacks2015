Param([string] $sblib,
	[string] $connectionString,
	[string] $queue)
	
write-host "========= Resetting Queues... ========="
[System.Reflection.Assembly]::LoadFrom($sblib)

$nm = [Microsoft.ServiceBus.NamespaceManager]::CreateFromConnectionString($connectionString)

if ($nm.QueueExists($queue))
{
	$nm.DeleteQueue($queue)
}
$nm.CreateQueue($queue)

write-host "Resetting Queues done!"