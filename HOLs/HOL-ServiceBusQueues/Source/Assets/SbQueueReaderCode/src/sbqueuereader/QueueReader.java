package sbqueuereader;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.util.Enumeration;
import java.util.Hashtable;

import javax.jms.Connection;
import javax.jms.ConnectionFactory;
import javax.jms.Destination;
import javax.jms.JMSException;
import javax.jms.MapMessage;
import javax.jms.Message;
import javax.jms.MessageConsumer;
import javax.jms.MessageListener;
import javax.jms.Session;
import javax.naming.Context;
import javax.naming.InitialContext;

public class QueueReader implements MessageListener {
    private Connection connection;
    private Session receiveSession;
    private MessageConsumer receiver;

    public QueueReader() throws Exception {
        // Configure JNDI environment
        Hashtable<String, String> env = new Hashtable<String, String>();
        env.put(Context.INITIAL_CONTEXT_FACTORY, "org.apache.qpid.amqp_1_0.jms.jndi.PropertiesFileInitialContextFactory");
        env.put(Context.PROVIDER_URL, "servicebus.properties");
        Context context = new InitialContext(env);

        // Lookup ConnectionFactory and Queue
        ConnectionFactory cf = (ConnectionFactory) context.lookup("SBCF");
        Destination queue = (Destination) context.lookup("QUEUE");

        // Create Connection
        connection = cf.createConnection();
        
        // Create receiver-side Session, MessageConsumer,and MessageListener
        receiveSession = connection.createSession(false, Session.CLIENT_ACKNOWLEDGE);
        receiver = receiveSession.createConsumer(queue);
        receiver.setMessageListener(this);
        connection.start();
    }
    
	public static void main(String[] args) {
		try {
            QueueReader queueReader = new QueueReader();
            System.out.println("Press [enter] to quit.");
            BufferedReader commandLine = new java.io.BufferedReader(new InputStreamReader(System.in));

            while (true) {
                String s = commandLine.readLine();
                if (s != null) {
                	queueReader.close();
                    System.exit(0);
                } else {
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
	}

    public void close() throws JMSException {
        connection.close();
    }

    public void onMessage(Message message) {
		 try {
			 System.out.println("Received message with Message with ID = " + message.getJMSMessageID());
		     if (message instanceof MapMessage) {
		    	 @SuppressWarnings("unchecked")
				Enumeration<String> properties = (Enumeration<String>) ((MapMessage)message).getMapNames();
		    	 while(properties.hasMoreElements()){
		    		 String param = (String) properties.nextElement();
		    		 System.out.println("Body property " + param + ": " + ((MapMessage)message).getString(param));
		    	 }
		     }
		     System.out.println("Message Property Urgent = " + message.getStringProperty("Urgent"));
			 System.out.println("Message Property Priority = " + message.getStringProperty("Priority"));
		     message.acknowledge();
		 } catch (Exception e) {
		     e.printStackTrace();
		 }
    }
}

