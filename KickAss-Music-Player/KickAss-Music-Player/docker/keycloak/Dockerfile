FROM jboss/keycloak

ENV TZ America/Montreal

RUN mkdir -p /opt/jboss/keycloak/configs

# Copy HydroSolution's realm configuration on the container
ADD ["config/Kickass-media-player-realm.json", "/opt/jboss/keycloak/configs/Kickass-media-player-realm.json"]
ADD ["config/Kickass-media-player-users.json", "/opt/jboss/keycloak/configs/Kickass-media-player-users.json"]

# Import HydroSolution configuration
CMD ["-b", "0.0.0.0", "-Dkeycloak.migration.action=import", "-Dkeycloak.migration.provider=dir", "-Dkeycloak.migration.dir=/opt/jboss/keycloak/configs", "-Dkeycloak.migration.strategy=OVERWRITE_EXISTING"]