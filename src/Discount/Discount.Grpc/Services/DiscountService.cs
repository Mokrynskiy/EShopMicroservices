using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon is null)
                coupon = new Coupon { ProductName = "No Discont", Amount = 0, Description = "No Description" };

            logger.LogInformation("Discount is for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "InvalidRequest"));

            dbbContext.Coupons.Add(coupon);
            await dbbContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully created. ProductName : {productName}", coupon.ProductName);

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "InvalidRequest"));

            dbbContext.Coupons.Update(coupon);
            await dbbContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully updated. ProductName : {productName}", coupon.ProductName);

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found"));

            dbbContext.Coupons.Remove(coupon);
            await dbbContext.SaveChangesAsync();
            logger.LogInformation("Discount is successfully created. ProductName : {productName}", request.ProductName);
            return new DeleteDiscountResponse { Success = true };
        }
    }
}
