package net.wulfen.httpnotifier.utils;

import jenkins.model.Jenkins;

public class PathUtil {

    public static String absoluteUrl(String path) {
        String uri = Jenkins.getInstance().getRootUrl();
        if (uri != null) {
            uri += path;
        }
        return uri;
    }

}
