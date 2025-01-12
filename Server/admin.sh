curl -X POST http://localhost:5000/admin/integrator -H "Authorization:admin123" -H "Content-Type:application/json" -d '{"ApiKey":"integrator123","Role":"Base"}' -v
curl -X GET http://localhost:5000/api/v1/reminders/byid -H "Authorization:integrator123" -H "Content-Type:application/json" -d '{"Id":"1"}' -v
