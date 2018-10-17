package net.wulfen.httpnotifier.models;

import hudson.model.Run;
import net.wulfen.httpnotifier.utils.PathUtil;

public class Build {

    private String name;
    private long duration;
    private String url;
    private State state;
    private Result result;

    public Build(Run run, State state) {
        this.name = run.getDisplayName();
        this.duration = run.getDuration();
        this.url = PathUtil.absoluteUrl(run.getUrl());
        this.state = state;

        hudson.model.Result _result = run.getResult();

        if (_result.equals(hudson.model.Result.ABORTED)) {
            this.result = Result.Aborted;
        }
        else if (_result.equals(hudson.model.Result.FAILURE)) {
            this.result = Result.Failure;
        }
        else if (_result.equals(hudson.model.Result.NOT_BUILT)) {
            this.result = Result.NotBuild;
        }
        else if (_result.equals(hudson.model.Result.SUCCESS)) {
            this.result = Result.Success;
        }
        else if (_result.equals(hudson.model.Result.UNSTABLE)) {
            this.result = Result.Unstable;
        }

    }

    public String getName() {
        return name;
    }

    public Long getDuration() {
        return duration;
    }

    public String getUrl() {
        return url;
    }

    public State getState() {
        return state;
    }

    public Result getResult() {
        return result;
    }
}
