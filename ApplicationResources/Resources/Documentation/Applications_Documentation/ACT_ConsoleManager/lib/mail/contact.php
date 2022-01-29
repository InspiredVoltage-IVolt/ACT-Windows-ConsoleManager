<?php	
// Loads SwiftMailer library
require_once('swift_required.php');	
$form = $_POST['form'];
// Send email
$transport = Swift_MailTransport::newInstance();
$mailer = Swift_Mailer::newInstance($transport);
$message = Swift_Message::newInstance()
	->setSubject($form['subject'])
	->setTo(array('lukas@ait.sk' => 'Lukas Vinclav'))
	->setFrom(array($form['mail'] => $form['name']))
	->setBody($form['message']);
$result = $mailer->send($message);	
echo $result;
?>