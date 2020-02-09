import React from "react";

export default ({ text }) => {
  return (
    <div
      style={{
        fontSize: 70,
        fontWeight: 700,
        display: "flex",
        height: "100%",
        justifyContent: "center",
        marginTop: "100px"
      }}
    >
      <span>{text}</span>
    </div>
  );
};
