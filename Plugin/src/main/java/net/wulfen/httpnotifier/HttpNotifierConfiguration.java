package net.wulfen.httpnotifier;

import hudson.Extension;
import hudson.model.*;
import jenkins.model.GlobalConfiguration;
import net.sf.json.JSONObject;
import org.kohsuke.stapler.DataBoundConstructor;
import org.kohsuke.stapler.StaplerRequest;

import java.util.logging.Logger;

public class HttpNotifierConfiguration extends JobProperty<AbstractProject<?, ?>> {

    private static final Logger LOGGER = Logger.getLogger(HttpNotifierConfiguration.class.getName());

    private final String endPoint;

    @DataBoundConstructor
    public HttpNotifierConfiguration(String endPoint) {
        this.endPoint = endPoint;
    }

    public String getEndPoint() {
        return endPoint;
    }

    public String getGlobalEndPoint() {
        return getDescriptor().getEndPoint();
    }

    @Override
    public HttpNotifierDescriptor getDescriptor() {
        return (HttpNotifierDescriptor) super.getDescriptor();
    }

    @Extension
    public static final class HttpNotifierDescriptor extends JobPropertyDescriptor {

        private String endPoint;

        public HttpNotifierDescriptor() {
            load();
        }

        @Override
        public boolean isApplicable(@SuppressWarnings("rawtypes") Class<? extends Job> jobType) {
            return true;
        }

        @Override
        public boolean configure(StaplerRequest req, JSONObject json) throws FormException {
            endPoint = json.getString("endPoint");
            save();
            return super.configure(req, json);
        }

        @Override
        public String getDisplayName() {
            return "HTTP Notifier";
        }

        public String getEndPoint() {
            return endPoint;
        }
    }
    
}