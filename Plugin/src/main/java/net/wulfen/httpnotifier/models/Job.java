package net.wulfen.httpnotifier.models;

import hudson.model.Run;
import net.wulfen.httpnotifier.utils.PathUtil;

public class Job {
    
    private String name;
    private String url;
    
    public Job(Run run) {
        this.name = run.getParent().getDisplayName();
        this.url = PathUtil.absoluteUrl(run.getParent().getUrl());
    }
    
    public String getName() {
        return name;
    }
    
    public String getUrl() {
        return url;
    }
    
}