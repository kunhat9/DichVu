USE master
GO
DROP DATABASE DB_SERVICES
GO
CREATE DATABASE DB_SERVICES
GO
USE DB_SERVICES
GO
/******************************** CREATE ********************************/
GO
CREATE TABLE TB_USERS					-- Người dùng
(
	UserId int IDENTITY PRIMARY KEY
	, Username nvarchar(30)
	, UserPassword nvarchar(30)
	, UserFullName NVARCHAR(255)
	, UserPhone nvarchar(20)			-- Số điện thoại
	, UserEmail nvarchar(50)			-- Email
	, UserAddress nvarchar(MAX)			-- Địa chỉ
	, UserType nvarchar(20)				-- Phân loại: ADMIN / STAFF
	, UserStatus nvarchar(20)			-- Trạng thái
	, UserNote nvarchar(100)			-- Ghi chú
)
GO
CREATE TABLE TB_TYPES					-- Loại dịch vụ (Cấu hình)
(
	TypeCode varchar(10) PRIMARY KEY	-- Loại dịch vụ: GTV, PH, VPR, VPA, TVDN, KTDN
	, TypeName nvarchar(50)				-- Tên dịch vụ: GÓI THÀNH VIÊN, PHÒNG HỌP, VP RIÊNG, VP ẢO, TVDN, KTDN
	, TypeStatus varchar(20)			-- Trạng thái: A: Active, D: Deactive
	, TypeType varchar(20)				-- Kiểu loại: VP: Văn phòng, SK: Sự kiện
)
GO
CREATE TABLE TB_TYPE_DETAILS			-- Chi tiết dịch vụ (Cấu hình)
(
	TypeDetailId int IDENTITY PRIMARY KEY
	, TypeDetailName nvarchar(50)		-- Key
	, TypeDetailType varchar(50)		-- Loại giá trị: STRING, BOOL, INT

	, TypeDetailTypeCode varchar(10)
	, CONSTRAINT FK_TypeDetailTypeCode FOREIGN KEY (TypeDetailTypeCode) REFERENCES TB_TYPES(TypeCode)
)
GO
CREATE TABLE TB_SERVICES				-- Dịch vụ
(
	ServiceId int IDENTITY PRIMARY KEY

	, ServiceName nvarchar(50)			-- Tên dịch vụ
	, ServicePrice decimal(18,2)		-- Giá tiền
	, ServiceUnit nvarchar(30)			-- Đơn vị
	, ServiceBase nvarchar(50)			-- Cơ sở
	, ServiceContent nvarchar(MAX)		-- Giới thiệu
	, ServiceStatus varchar(20)			-- Trạng thái: A: Active, D: Deactive
	
	, ServiceTypeCode varchar(10)
	, CONSTRAINT FK_ServiceTypeCode FOREIGN KEY (ServiceTypeCode) REFERENCES TB_TYPES(TypeCode)
)
GO
CREATE TABLE TB_SERVICE_DETAILS			-- Chi tiết dịch vụ
(
	SrvDetailId int IDENTITY PRIMARY KEY
	, SrvDetailValue nvarchar(50)		-- Value

	, SrvDetailServiceId int
	, CONSTRAINT FK_SrvDetailServiceId FOREIGN KEY (SrvDetailServiceId) REFERENCES TB_SERVICES(ServiceId)
	, SrvTypeDetailId int
	, CONSTRAINT FK_SrvTypeDetailId FOREIGN KEY (SrvTypeDetailId) REFERENCES TB_TYPE_DETAILS(TypeDetailId)
)
GO
CREATE TABLE TB_MENUS					-- Thực đơn
(
	MenuId int IDENTITY PRIMARY KEY
	, MenuName nvarchar(50)				-- Tên thực đơn (Combo A)
	, MenuPrice decimal					-- Giá tiền
	, MenuNum int						-- Số người / 1 bàn
	, MenuNote nvarchar(MAX)			-- Ghi chú
	, MenuStatus varchar(20)			-- Trạng thái: A: Active, D: Deactive
	
	, MenuServiceId int
	, CONSTRAINT FK_MenuServiceId FOREIGN KEY (MenuServiceId) REFERENCES TB_SERVICES(ServiceId)
)
GO
CREATE TABLE TB_MENU_GROUPS				-- Nhóm món ăn
(
	MgroupId int IDENTITY PRIMARY KEY
	, MgroupName nvarchar(50)			-- Tên nhóm (Súp, Nộm)
	, MgroupSelected int DEFAULT(1)		-- Số món được chọn
	
	, MgroupMenuId int
	, CONSTRAINT FK_MgroupMenuId FOREIGN KEY (MgroupMenuId) REFERENCES TB_MENUS(MenuId)
)
GO
CREATE TABLE TB_MENU_DETAILS			-- Món ăn
(
	MdetailId int IDENTITY PRIMARY KEY
	, MdetailName nvarchar(50)			-- Tên món ăn (Súp gà)
	
	, MdetailMgroupId int
	, CONSTRAINT FK_MdetailMgroupId FOREIGN KEY (MdetailMgroupId) REFERENCES TB_MENU_GROUPS(MgroupId)
)
GO
CREATE TABLE TB_FILES					-- Ảnh
(
	FileId int IDENTITY PRIMARY KEY
	, FileOrg nvarchar(200)				-- Tên gốc
	, FilePath nvarchar(250)			-- Đường dẫn
	, FileData nvarchar(MAX)			-- Base64
	, FileStatus nvarchar(20)			-- Trạng thái
	, FileType nvarchar(50)				-- Bảng: SERVICE(1) / BLOG(1+) / PRODUCT(1+) / SLIDER(1)
	, FileReferenceId nvarchar(50)		-- Khóa ngoại
)
GO
CREATE TABLE TB_REGISTERS				-- Đăng ký dịch vụ
(
	RegisterId int IDENTITY PRIMARY KEY
	
	, RegisterUserName nvarchar(50)		-- Tên
	, RegisterUserEmail nvarchar(50)	-- Email
	, RegisterUserPhone nvarchar(20)	-- Số điện thoại
	, RegisterUserNote nvarchar(MAX)	-- Ghi chú đơn hàng

	, RegisterDateCreate date			-- Ngày đăng ký
	, RegisterDateBegin date			-- Ngày bắt đầu / Ngày tổ chức
	, RegisterDateNumber int			-- Số ngày thuê
	, RegisterStatus nvarchar(20)		-- Trạng thái: N:New
	
	, RegisterServiceName nvarchar(50)	-- Tên dịch vụ
	, RegisterServicePrice decimal(18,2)-- Giá tiền
	, RegisterServiceUnit nvarchar(30)	-- Đơn vị
	, RegisterServiceBase nvarchar(50)	-- Cơ sở
	, RegisterVoucherNum decimal		-- Số tiền / Phần trăm
	, RegisterVoucherType nvarchar(20)	-- Loại KM: M: giảm tiền / P: phần trăm
	, RegisterMenuNumber int			-- Số lượng bàn được chọn
	, RegisterMenuPrice decimal			-- Giá tiền
	
	, RegisterMenuId int				-- Thực đơn được chọn
	, CONSTRAINT FK_RegisterMenuId FOREIGN KEY (RegisterMenuId) REFERENCES TB_MENUS(MenuId)
	, RegisterServiceId int
	, CONSTRAINT FK_RegisterServiceId FOREIGN KEY (RegisterServiceId) REFERENCES TB_SERVICES(ServiceId)
	, RegisterUserId int					-- Người thao tác (hiện tại)
	, CONSTRAINT FK_RegisterUserId FOREIGN KEY (RegisterUserId) REFERENCES TB_USERS(UserId)
)
GO
CREATE TABLE TB_REGISTER_DETAILS		-- Chi tiết đăng ký
(
	RdetailId int IDENTITY PRIMARY KEY

	, RdetailMdetailId int				-- Món ăn được chọn
	, CONSTRAINT FK_RdetailMdetailId FOREIGN KEY (RdetailMdetailId) REFERENCES TB_MENU_DETAILS(MdetailId)
	, RdetailRegisterId int				-- đăng kí theo bản nào
	, CONSTRAINT FK_RdetailRegisterId FOREIGN KEY (RdetailRegisterId) REFERENCES TB_REGISTERS(RegisterId)
)
GO
--ALTER TABLE TB_REGISTER_DETAILS ADD RdetailRegisterId int  -- đăng kí theo bản nào
--GO
CREATE TABLE TB_REGISTER_HISTORIES		-- Lịch sử thao tác
(
	HistoryId int IDENTITY PRIMARY KEY
	, HistoryNote nvarchar(MAX)			-- Lý do
	, HistoryDate date					-- Ngày thay đổi
	, HistoryStatus nvarchar(20)		-- Trạng thái

	, HistoryUserId int					-- Người thao tác
	, CONSTRAINT FK_HistoryUserId FOREIGN KEY (HistoryUserId) REFERENCES TB_USERS(UserId)
)
GO
CREATE TABLE TB_BLOGS					-- Tin tức, bài viết
(
	BlogId int IDENTITY PRIMARY KEY
	, BlogName nvarchar(50)				-- Tên bài viết
	, BlogContent nvarchar(MAX)			-- Nội dung
	, BlogDateCreate date				-- Ngày tạo
	, BlogType nvarchar(20)				-- Loại bài viết (KM: Khuyến mại, TT: Thị trường)
	, BlogIsShow bit					-- Trạng thái hiển thị

	, BlogUserId int					-- Người viết
	, CONSTRAINT FK_BlogUserId FOREIGN KEY (BlogUserId) REFERENCES TB_USERS(UserId)
)
GO
CREATE TABLE TB_SLIDERS					-- Slider
(
	SliderId int IDENTITY PRIMARY KEY
	, SliderName nvarchar(50)			-- Tên bài viết
	, SliderContent nvarchar(MAX)		-- Nội dung
	, SliderDateCreate date				-- Ngày tạo
	, SliderIsShow bit					-- Trạng thái hiển thị

	, SliderUserId int					-- Người viết
	, CONSTRAINT FK_SliderUserId FOREIGN KEY (SliderUserId) REFERENCES TB_USERS(UserId)
)
GO
CREATE TABLE TB_VOUCHERS				-- Khuyến mại
(
	VoucherCode varchar(20) PRIMARY KEY	-- MÃ KM
	, VoucherDateCreate date			-- Ngày tạo
	, VoucherDateExpired date			-- Ngày hết hạn
	, VoucherNum decimal				-- Số tiền / Phần trăm
	, VoucherType nvarchar(20)			-- Loại KM: M: giảm tiền / P: phần trăm
	, VoucherLimited int				-- Số lần giới hạn
	, VoucherState varchar(20)					-- Trạng thái A/D
	, VoucherNote nvarchar(MAX)			-- Mô tả
	
	, VoucherServiceId int				-- Dịch vụ áp dụng
	, CONSTRAINT FK_VoucherServiceId FOREIGN KEY (VoucherServiceId) REFERENCES TB_SERVICES(ServiceId)
)
GO
/******************************** INSERT ********************************/
GO
INSERT INTO TB_USERS(Username, UserPassword, UserType, UserStatus) VALUES ('admin', '1', 'ADMIN', 'A')
GO
INSERT INTO TB_TYPES(TypeCode, TypeName, TypeStatus, TypeType)
VALUES ('GTV', N'GÓI THÀNH VIÊN', 'A', 'VP')
	, ('PH', N'PHÒNG HỌP', 'A', 'VP')
GO
INSERT INTO TB_TYPE_DETAILS(TypeDetailName, TypeDetailType, TypeDetailTypeCode)
VALUES (N'Thời gian làm việc', 'STRING', 'GTV')
	, (N'Phòng họp', 'BOOL', 'GTV')
	, (N'In ấn miễn phí', 'BOOL', 'GTV')
	, (N'Wifi tốc độ cao', 'BOOL', 'GTV')
	, (N'Lễ tân, nhận bưu phẩm', 'BOOL', 'GTV')
	, (N'Chỗ ngồi cố định', 'BOOL', 'GTV')
	, (N'Tủ để đồ miễn phí', 'BOOL', 'GTV')
	, (N'Trà, café', 'BOOL', 'GTV')
	, (N'Ghế Massage, Bàn Bi A, Bi Lắc, Bóng Bàn, ...', 'BOOL', 'GTV')
	, (N'Bếp và khu tiếp khách, vườn ngoài trời', 'BOOL', 'GTV')
	, (N'Kết nối BisHub', 'BOOL', 'GTV')
	, (N'Sự kiện BisHub', 'BOOL', 'GTV')
	, (N'3 gói Trải nghiệm miễn phí 1 ngày cho bạn bè', 'BOOL', 'GTV')
	, (N'Ưu đãi từ đối tác', 'BOOL', 'GTV')
	, (N'Thư viện sách', 'BOOL', 'GTV')
GO
INSERT INTO TB_TYPE_DETAILS(TypeDetailName, TypeDetailType, TypeDetailTypeCode)
VALUES (N'Sức chứa', 'STRING', 'PH')
	, (N'Thiết bị trình chiếu', 'BOOL', 'PH')
	, (N'Bảng Flipchart', 'BOOL', 'PH')
	, (N'HDMI', 'BOOL', 'PH')
	, (N'Loa, Micro hiện đại', 'BOOL', 'PH')
	, (N'Diện tích', 'STRING', 'PH')
	, (N'Tea break ngọt', 'STRING', 'PH')
	, (N'Trà cafe', 'STRING', 'PH')
GO
/*
INSERT INTO TB_TYPE_DETAILS(TypeDetailName, TypeDetailType, TypeDetailTypeCode)
VALUES (N'Thời gian làm việc', 'STRING', 'VPR')
	, (N'Lễ tân', 'BOOL', 'VPR')
	, (N'Phòng họp', 'STRING', 'VPR')
	, (N'Số chỗ ngồi', 'INT', 'VPR')
	, (N'Trà café hòa tan', 'BOOL', 'VPR')
	, (N'Thư viện sách', 'BOOL', 'VPR')
	, (N'Ghế Massage, Bàn Bi A, Bi Lắc, ...', 'BOOL', 'VPR')
	, (N'Dịch vụ in ấn', 'STRING', 'VPR')
	, (N'Điện nước miễn phí', 'BOOL', 'VPR')
	, (N'Biển tên Công ty', 'BOOL', 'VPR')
	, (N'Dịch vụ bảo vệ', 'BOOL', 'VPR')
	, (N'Tiếp nhận bưu phẩm', 'BOOL', 'VPR')
	, (N'Địa chỉ giao dịch', 'BOOL', 'VPR')
	, (N'Địa chỉ đăng ký KD', 'BOOL', 'VPR')
GO
INSERT INTO TB_TYPE_DETAILS(TypeDetailName, TypeDetailType, TypeDetailTypeCode)
VALUES (N'Giá', 'STRING', 'VPA')
	, (N'Thời gian làm việc', 'STRING', 'VPA')
	, (N'Lễ tân', 'BOOL', 'VPA')
	, (N'Phòng họp', 'STRING', 'VPA')
	, (N'Số chỗ ngồi', 'INT', 'VPA')
	, (N'Trà café hòa tan', 'BOOL', 'VPA')
	, (N'Thư viện sách', 'BOOL', 'VPA')
	, (N'Ghế Massage, Bi A, Bi Lắc, ...', 'BOOL', 'VPA')
	, (N'Dịch vụ in ấn', 'STRING', 'VPA')
	, (N'Điện nước miễn phí', 'BOOL', 'VPA')
	, (N'Biển tên Công ty', 'BOOL', 'VPA')
	, (N'Dịch vụ bảo vệ', 'BOOL', 'VPA')
	, (N'Tiếp nhận bưu phẩm', 'BOOL', 'VPA')
	, (N'Địa chỉ giao dịch', 'BOOL', 'VPA')
	, (N'Địa chỉ đăng ký KD', 'BOOL', 'VPA')
GO
*/
INSERT INTO TB_SERVICES(ServiceName, ServicePrice, ServiceUnit, ServiceBase, ServiceStatus, ServiceContent, ServiceTypeCode)
VALUES (N'GÓI THÀNH VIÊN NGÀY', 50000, N'VND / 1 Ngày', N'Xã Đàn', 'A', N'Với 1 hoặc 3 ngày làm việc linh hoạt trong tháng, phù hợp với những bạn đi công tác tại Hà Nội, hoặc đổi không khí làm việc. Chi phí từ 60k, tiết kiệm mà được sử dụng 700m2 diện tích chung.', 'GTV')
	, (N'GÓI THÀNH VIÊN NGÀY', 80000, N'VND / 1 Ngày', N'MIPEC', 'A', N'Với 1 hoặc 3 ngày làm việc linh hoạt trong tháng, phù hợp với những bạn đi công tác tại Hà Nội, hoặc đổi không khí làm việc. Chi phí từ 60k, tiết kiệm mà được sử dụng 700m2 diện tích chung.', 'GTV')
	, (N'GÓI THÀNH VIÊN 3 NGÀY', 130000, N'VND / 3 Ngày', N'Xã Đàn', 'A', N'Bạn hay phải đi công tác, mỗi tháng bạn chỉ làm việc 1-2 tuần tại Hà Nội, gói tuần giúp bạn tiết kiệm chi phí mà vẫn đáp ứng đầy đủ 1 văn phòng cao cấp với 700m2 diện tích chung.', 'GTV')
	, (N'GÓI THÀNH VIÊN 3 NGÀY', 200000, N'VND / 3 Ngày', N'MIPEC', 'A', N'Bạn hay phải đi công tác, mỗi tháng bạn chỉ làm việc 1-2 tuần tại Hà Nội, gói tuần giúp bạn tiết kiệm chi phí mà vẫn đáp ứng đầy đủ 1 văn phòng cao cấp với 700m2 diện tích chung.', 'GTV')
	, (N'GÓI THÀNH VIÊN LINH HOẠT', 1200000, N'VND / 1 Người / Tháng', N'Xã Đàn', 'A', N'Bạn hay nhóm của bạn cần không gian sáng tạo, tiện nghi và không phải lo về những thứ khác => gói linh hoạt giúp bạn tiết kiệm chi phí => bạn được sử dụng đầy đủ 700m2 chung.', 'GTV')
	, (N'GÓI THÀNH VIÊN LINH HOẠT', 1600000, N'VND / 1 Người / Tháng', N'MIPEC', 'A', N'Bạn hay nhóm của bạn cần không gian sáng tạo, tiện nghi và không phải lo về những thứ khác => gói linh hoạt giúp bạn tiết kiệm chi phí => bạn được sử dụng đầy đủ 700m2 chung.', 'GTV')
GO
INSERT INTO TB_SERVICES(ServiceName, ServicePrice, ServiceUnit, ServiceBase, ServiceStatus, ServiceContent, ServiceTypeCode)
VALUES (N'PHÒNG HỌP M01', 200000, N'1h', N'BisHub Mipec Tây Sơn', 'A', N'', 'PH')
	, (N'PHÒNG HỌP M03', 600000, N'1h', N'BisHub Mipec Tây Sơn', 'A', N'', 'PH')
GO
/******************************** NOTE ********************************
WEB ADMIN
	1, Trang chủ
	2, Quản lý phân quyền
	3, Quản lý dịch vụ
	4, Quản lý thành viên
	5, Quản lý bài viết

	6, Giải trí - thể thao

WEB CLIENT
	1, Trang chủ
	2, Dịch vụ
		Thông tin
		Đăng ký
	3, Bài viết

	4, Giỏ hàng
	5, Thanh toán
	6, Sản phẩm
		Danh sách
		Chi tiết

	7, Kết nối FB
*/