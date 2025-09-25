// src/App.js
import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

// صفحات عمومی
import Home from "./pages/Home";

// صفحات مدیریت ملک‌ها
import PropertiesDashboard from "./pages/PropertiesDashboard";
import CreateProperty from "./pages/CreateProperty";
import EditProperty from "./pages/EditProperty";

function App() {
  return (
    <Router>
      <Routes>
        {/* صفحات عمومی */}
        <Route path="/" element={<Home />} />

        {/* مدیریت ملک‌ها */}
        <Route path="/properties" element={<PropertiesDashboard />} />
        <Route path="/properties/create" element={<CreateProperty />} />
        <Route path="/properties/edit/:id" element={<EditProperty />} />
      </Routes>
    </Router>
  );
}

export default App;
