// src/pages/Dashboard.js
import React, { useEffect, useState } from "react";

function Dashboard() {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch("http://localhost:8000/api/v1/bookings")
      .then((res) => res.json())
      .then((result) => {
        setData(result);
        setLoading(false);
      })
      .catch((err) => {
        console.error("Error fetching data:", err);
        setLoading(false);
      });
  }, []);

  return (
    <div className="container mt-5">
      <h2 className="mb-4 text-center">📊 Dashboard</h2>

      {loading ? (
        <div className="text-center">
          <div className="spinner-border text-primary" role="status"></div>
          <p className="mt-2">در حال بارگذاری...</p>
        </div>
      ) : (
        <table className="table table-striped table-hover shadow">
          <thead className="table-dark">
            <tr>
              <th>#</th>
              <th>نام</th>
              <th>مهمان ها</th>
              <th>قیمت</th>
            </tr>
          </thead>
          <tbody>
            {data.map((user, index) => (
              <tr key={user.id}>
                <td>{index + 1}</td>
                <td>{user.Guests}</td>
                <td>{user.TotalPrice}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default Dashboard;
