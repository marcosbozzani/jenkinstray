package net.wulfen.httpnotifier;

import hudson.Extension;
import hudson.model.*;
import hudson.model.listeners.RunListener;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.util.logging.Logger;

import jenkins.model.Jenkins;
import net.wulfen.httpnotifier.models.Notification;
import net.wulfen.httpnotifier.models.State;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.core.JsonProcessingException;
import org.apache.http.impl.client.HttpClientBuilder;

@Extension
public class HttpNotifier extends RunListener<Run> {

    private static final Logger LOGGER = Logger.getLogger(HttpNotifier.class.getName());

    @Override
	public void onStarted(Run run, TaskListener taskListener) {
		sendNotification(run, State.Started, taskListener);
		super.onStarted(run, taskListener);
	}

	@Override
	public void onCompleted(Run run, TaskListener taskListener) {
        sendNotification(run, State.Completed, taskListener);
		super.onCompleted(run, taskListener);
	}

	@Override
	public void onFinalized(Run run) {
        sendNotification(run, State.Finalized, null);
		super.onFinalized(run);
	}

    @Override
    public void onDeleted(Run run) {
        sendNotification(run, State.Deleted, null);
        super.onDeleted(run);
    }
    
    private void sendNotification(Run run, State state, TaskListener taskListener) {
        String endPoint = "";

        try {

            endPoint = getEndPoint(run);

            if (endPoint == null) {
                return;
            }

            Notification notification = new Notification(run, state);
            
            ObjectMapper mapper = new ObjectMapper();
            String json = mapper.writeValueAsString(notification);
            
            StringEntity entity = new StringEntity(json);
            entity.setContentType("application/json; charset=utf-8");
            
            HttpPost post = new HttpPost(endPoint);
            post.setEntity(entity);
            
            HttpClient httpclient =  HttpClientBuilder.create().build();
            httpclient.execute(post);

            info("Notification sent", state, endPoint, taskListener);
            
        }
        catch (JsonProcessingException ex) {
            error("Unable to process Json data", state, endPoint, ex, taskListener);
        }
        catch (UnsupportedEncodingException ex) {
            error("Unable to encode notification data", state, endPoint, ex, taskListener);
        } 
        catch (ClientProtocolException ex) {
            error("Unable to send notification", state, endPoint, ex, taskListener);
        }
        catch (IOException ex) {
            error("Unable to send notification", state, endPoint, ex, taskListener);
        }
        catch (Exception ex) {
            error("Unable to send notification", state, endPoint, ex, taskListener);
        }
    }

    private void info(String label, State state, String endPoint, TaskListener taskListener) {
        if (taskListener != null) {
            String message = getMessage(label, state, endPoint);
            taskListener.getLogger().print(message + "\n");
        }
    }

    private void error(String label, State state, String endPoint, Throwable exception, TaskListener taskListener) {
        String message = getMessage(label, state, endPoint);

        while (exception != null) {
            message += "\n\t" + exception;
            exception = exception.getCause();
        }

        LOGGER.info(message);
        if (taskListener != null) {
            taskListener.getLogger().print(message + "\n");
        }
    }

    private String getMessage(String label, State state, String endPoint) {
        return "[HTTP Notifier] " + label + "(State:" +  state + "; EndPoint: " + endPoint + ")";
    }

    private String getEndPoint(Run run) {
        HttpNotifierConfiguration configuration = (HttpNotifierConfiguration) run.getParent()
            .getProperty(HttpNotifierConfiguration.class);

        String endPoint = null;

        if (configuration != null) {
            endPoint = configuration.getEndPoint();

            if (endPoint == null || endPoint.isEmpty()) {
                endPoint = configuration.getGlobalEndPoint();
            }
        }
        else {
            HttpNotifierConfiguration.HttpNotifierDescriptor descriptor =
                    (HttpNotifierConfiguration.HttpNotifierDescriptor) Jenkins.getInstance()
                            .getDescriptor(HttpNotifierConfiguration.class);

            endPoint = descriptor.getEndPoint();
        }

        if (endPoint == null || endPoint.isEmpty()) {
            return null;
        }

        return endPoint;

    }

}