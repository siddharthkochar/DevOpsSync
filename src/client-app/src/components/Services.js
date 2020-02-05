import React from "react";
import map from "lodash/map";
import { Container, CardDeck, Card, CardImg } from "reactstrap";

const Services = ({ services }) => (
  <Container style={{ marginTop: 50 }}>
    <CardDeck style={{ justifyContent: "center" }}>
      {map(services, service => (
        <Card
          inverse
          key={service.id}
          outline
          style={{
            backgroundColor: service.color,
            borderColor: service.color,
            textAlign: "center",
            maxWidth: "25%",
            width: "auto",
            padding: 20,
            cursor: "pointer",
            border: "2px solid #000",
            borderRadius: "8px"
          }}
        >
          <a
            href={`https://localhost:5001/api/${service.serviceName}/initialize`}
          >
            <CardImg
              top
              style={{
                height: 150,
                margin: "auto",
                width: "auto"
              }}
              src={service.imageUrl || require("../logo.svg")}
              alt="Card image cap"
            />
          </a>
        </Card>
      ))}
    </CardDeck>
  </Container>
);

export default Services;
