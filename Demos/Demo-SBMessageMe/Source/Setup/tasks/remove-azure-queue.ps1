Param([string] $sblib,
	[string] $connectionString,
	[string] $queue)
	
write-host "========= Removing Queues... ========="
[System.Reflection.Assembly]::LoadFrom($sblib)

$nm = [Microsoft.ServiceBus.NamespaceManager]::CreateFromConnectionString($connectionString)

if ($nm.QueueExists($queue))
{
	$nm.DeleteQueue($queue)
}

write-host "Removing Queues done!"