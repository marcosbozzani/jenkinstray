package net.wulfen.httpnotifier.models;

import hudson.model.Run;

public class Notification {
        
    private Job job;
    private Build build;
    
    public Notification(Run run, State state) {
        this.job = new Job(run);
        this.build = new Build(run, state);
    }
    
    public Job getJob() {
        return job;
    }
    
    public Build getBuild() {
        return build;
    }
    
}